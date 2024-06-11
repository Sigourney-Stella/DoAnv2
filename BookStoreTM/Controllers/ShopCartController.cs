using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Azure.Core.HttpHeader;
using Newtonsoft.Json;
using System.Runtime.Serialization.DataContracts;


namespace BookStoreTM.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly AppDbContext _db;

        private static int orderIdCounter = 0;

        private List<ShopCart> carts = new List<ShopCart>();
        public ShopCartController(AppDbContext db)
        {
            _db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cartInSession = HttpContext.Session.GetString("My-Cart");
            if (cartInSession != null)
            {
                // nếu cartInSession không null thì gán dữ liệu cho biến carts
                // Chuyển san dữ liệu json
                carts = JsonConvert.DeserializeObject<List<ShopCart>>(cartInSession);
            }
            base.OnActionExecuting(context);
        }
        public IActionResult Index()
        {
            decimal total = 0;
            var count = 0;

            foreach (var item in carts)
            {
                total += item.Quantity * item.PriceSale;
                count++;
                // Lấy số lượng sản phẩm trong Products
                var product = _db.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    item.MaxQuantity = product.Quatity;
                }
            }
            ViewBag.Total = total;
            ViewBag.Count = count;
            return View(carts);
        }

        //thêm sản phẩm vào giỏ hàng
        public IActionResult Add(int id)
        {
            var items = carts.SingleOrDefault(p => p.ProductId == id);
            var p = _db.Products.FirstOrDefault(x => x.ProductId == id);
            if (carts.Any(c => c.ProductId == id))
            {
                carts.Where(c => c.ProductId == id).First().Quantity += 1;
            }
            else
            {
                var item = new ShopCart()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = (decimal)p.Price,
                    PriceSale = (decimal)p.PriceSale,
                    Quantity = 1,
                    ProductImg = p.Images,
                    TotalPrice = (decimal)p.PriceSale * 1
                };
                carts.Add(item);

                //đếm số sản phẩm vào giỏ hàng
                var totalQuantity = carts.Sum(c => c.Quantity);
                HttpContext.Session.SetInt32("CartTotalQuantity", totalQuantity);
            }

            //Lưu giỏ hàng vào Session
            HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));

            // Lưu giỏ hàng vào cơ sở dữ liệu nếu người dùng đã đăng nhập
            if(HttpContext.Session.GetString("Member") != null)
            {
                var dataMember = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                var userCart = _db.UserCarts.SingleOrDefault(x => x.CustomerID == dataMember.CustomerID && x.ProductId == id);
                if(userCart != null)
                {
                    userCart.Quantity += 1;
                }
                else
                {
                    var itemCart = new UserCart()
                    {
                        CustomerID = dataMember.CustomerID,
                        ProductId = id,
                        ProductName = p.ProductName,
                        Price = (decimal)p.Price,
                        PriceSale = (decimal)p.PriceSale,
                        Quantity = 1,
                        ProductImg = p.Images,
                        TotalPrice = (decimal)p.PriceSale * 1
                    };
                    _db.Add(itemCart);
                    _db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int id)
        {
            if (carts.Any(c => c.ProductId == id))
            {
                var item = carts.Where(c => c.ProductId == id).First();
                carts.Remove(item);
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));
                
                // xoá trong bảng usercart
                var userCarts = _db.UserCarts.Where(c => c.ProductId == id).ToList();
                _db.UserCarts.RemoveRange(userCarts);
                _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id, int quantity)
        {
            if (carts.Any(c => c.ProductId == id))
            {
                carts.Where(c => c.ProductId == id).First().Quantity = quantity;
                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));

                var totalQuantity = carts.Sum(c => c.Quantity);
                HttpContext.Session.SetInt32("CartTotalQuantity", totalQuantity);

                // Cập nhật cơ sở dữ liệu nếu người dùng đã đăng nhập
                if (HttpContext.Session.GetString("Member") != null)
                {
                    var dataMember = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                    var userCart = _db.UserCarts.SingleOrDefault(x => x.CustomerID == dataMember.CustomerID && x.ProductId == id);
                    if (userCart != null)
                    {
                        userCart.Quantity = quantity;
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("My-Cart");

            // Xóa giỏ hàng khỏi cơ sở dữ liệu nếu người dùng đã đăng nhập
            if (HttpContext.Session.GetString("Member") != null)
            {
                var dataMember = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                var userCarts = _db.UserCarts.Where(x => x.CustomerID == dataMember.CustomerID);
                _db.UserCarts.RemoveRange(userCarts);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Order(int id)
        {
            //nếu chưa đăng nhập
            if (HttpContext.Session.GetString("Member") == null)
            {
                return Redirect("/Customer/Login/?url=/Shopcart/orders");
            }
            else
            {
                var dataMember = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                ViewBag.Customer = dataMember;
                var userCart = _db.UserCarts.Where(x => x.CustomerID == dataMember.CustomerID);
                decimal total = 0;
                foreach (var item in userCart)
                {
                    if (item.PriceSale > 0 && item.PriceSale < item.Price)
                    {
                        total += item.Quantity * item.PriceSale;
                    }
                    else
                    {
                        total += item.Quantity * item.Price;
                    }
                    ViewBag.Total = total;
                    //phương thức thanh toán
                    var dataPay = _db.Payments.ToList();
                    ViewData["IdPayment"] = new SelectList(dataPay, "PaymentId", "PaymentName", 1);
                }
                return View(userCart);
            }
        }
        public async Task<IActionResult> OrderPay(IFormCollection form)
        {
            try
            {
                //thêm bảng orders
                var order = new OrderBook();
                order.ReceiveName = form["ReceiveName"];
                order.ReceiveAddress = form["AddresslReceive"];
                order.ReceivePhone = form["PhoneReceive"];
                order.Notes = form["Notes"];
                order.PaymentId = int.Parse(form["Idpayment"]);
                order.OrderDate = DateTime.Now;
                order.TransactStatusID = 1;
                var dataMember = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));
                order.CustomerID = dataMember.CustomerID;
                decimal total = 0;
                foreach (var item in carts)
                {
                    if (item.PriceSale > 0 && item.PriceSale < item.Price)
                    {
                        total += item.Quantity * item.PriceSale;
                    }
                    else
                    {
                        total += item.Quantity * item.Price;
                    }
                }

                order.TotalMoney = total;
                orderIdCounter++;
                //tạo OrderId
                var strOrderId = "DH";
                string times = DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss.fff");
                strOrderId += "." + times;
                order.CodeOrder = strOrderId;

                _db.Add(order);
                await _db.SaveChangesAsync();

                //Lấy id bảng orderbook
                var dataOrder = _db.OrderBooks.OrderByDescending(x => x.OrderId).FirstOrDefault();
                foreach (var items in carts)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.Code = dataOrder.CodeOrder;
                    orderDetails.OrderId = dataOrder.OrderId;
                    orderDetails.ProductId = items.ProductId;
                    orderDetails.Quatity = items.Quantity;
                    orderDetails.Price = items.Price;
                    orderDetails.TotalMoney = items.TotalPrice; // lấy ra giá tiền cuối ( price hoặc pricesale)

                    var quatityPro = _db.Products.Where(x => x.ProductId == items.ProductId).FirstOrDefault().Quatity;
                    _db.Products.Where(x => x.ProductId == items.ProductId).FirstOrDefault().Quatity = quatityPro - items.Quantity;
                    _db.Add(orderDetails);
                    await _db.SaveChangesAsync();
                }
                HttpContext.Session.Remove("My-Cart");
                //lấy giỏ hàng hiện tại
                var userCarts = _db.UserCarts.Where(x => x.CustomerID == dataMember.CustomerID).ToList();
                _db.UserCarts.RemoveRange(userCarts);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Bạn đã thanh toán thành công!" });
            }
            catch (Exception ex)
            {
               // throw;
                return Json(new { success = true, message = "Đã xảy ra lỗi khi thanh toán. Vui lòng thử lại." });
            }
           
            //return RedirectToAction("Index", "ShopCart");
        }
    }
}
