

using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace BookStoreTM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Login(string url)
        {
            string dataJson = HttpContext.Session.GetString("Member");
            if (dataJson != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(Customer model, string urlAction)
        {
            var pass = Utilitties.Utils.GetSHA26Hash(model.Password);
            var data = _context.Customers.Where(x => x.IsActive == true).Where(x => x.Email.Equals(model.Email))
                .FirstOrDefault(x => x.Password.Equals(pass));
            var dataLogin = data.ToJson();
            if (data != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("Member", dataLogin);

                // Tải giỏ hàng từ cơ sở dữ liệu vào session
                var userCarts = _context.UserCarts.Where(x => x.CustomerID == data.CustomerID).ToList();

                string cartSessionJson = HttpContext.Session.GetString("My-Cart");
                if (!string.IsNullOrEmpty(cartSessionJson))
                {
                    List<ShopCart> cartSession = JsonConvert.DeserializeObject<List<ShopCart>>(cartSessionJson);
                    // kiểm tra session có giỏ hàng chưa
                    if (cartSession != null)
                    {
                        // kiểm tra trong giỏ hàng của session có sản phẩm đó hay chưa
                        foreach (var item in cartSession)
                        {
                            var checkItem = userCarts.Where(x => x.ProductId == item.ProductId).Any();
                            if (!checkItem)
                            {
                                var itemCart = new UserCart()
                                {
                                    CustomerID = data.CustomerID,
                                    ProductId = item.ProductId,
                                    ProductName = item.ProductName,
                                    Price = (decimal)item.Price,
                                    PriceSale = (decimal)item.PriceSale,
                                    Quantity = item.Quantity,
                                    ProductImg = item.ProductImg,
                                    TotalPrice = (decimal)item.PriceSale * item.Quantity
                                };
                                _context.Add(itemCart);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

                //đẩy sản phẩm từ giỏ hàng lên Session
                userCarts = _context.UserCarts.Where(x => x.CustomerID == data.CustomerID).ToList();
                var carts = userCarts.Select(uc => new ShopCart
                {
                    ProductId = uc.ProductId,
                    ProductName = uc.ProductName,
                    Price = uc.Price,
                    PriceSale = uc.PriceSale,
                    Quantity = uc.Quantity,
                    ProductImg = uc.ProductImg,
                    TotalPrice = uc.PriceSale * uc.Quantity
                }).ToList();


                HttpContext.Session.SetString("My-Cart", JsonConvert.SerializeObject(carts));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu khôg đúng! Vui lòng nhập lại");
                return View(model);
            }
        }
        public IActionResult Registy()
        {
            string dataJson = HttpContext.Session.GetString("Member");
            if (dataJson != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
            //Customer model = new Customer();
            //return View(model);
        }
        [HttpPost]
        public IActionResult Registy(Customer model)
        {
            try
            {
                var taiKhoan = _context.Customers.Where(x => x.Email == model.Email).FirstOrDefault();
                if (taiKhoan == null)
                {
                    var pass = Utilitties.Utils.GetSHA26Hash(model.Password);
                    model.Password = pass;
                    model.CreateDate = DateTime.Now;
                    model.IsActive = true;
                    _context.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Customer");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng! Vui lòng sử dụng email khác");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Registy");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Member");
            HttpContext.Session.Remove("My-Cart");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                // Lấy đối tượng Customer hiện tại từ cơ sở dữ liệu
                var customer = _context.Customers.Find(model.CustomerID);
                if (customer == null)
                {
                    return NotFound();
                }
                // Cập nhật thông tin từ form vào đối tượng Customer
                customer.Fullname = model.Fullname;
                customer.Email = model.Email;
                customer.Address = model.Address;
                customer.Phone = model.Phone;
                customer.Brithday = model.Brithday;
                customer.UpdateDate = DateTime.Now;

                _context.SaveChanges();

                //Lưu lại thông tin vào session
                var updatedCustomer = JsonConvert.SerializeObject(customer);
                HttpContext.Session.SetString("Member", updatedCustomer);

                return RedirectToAction("Detail");
            }
            return View("Detail", model);
        }

        //xem thông tin đơn tài khoản
        public IActionResult Detail()
        {
            string dataJson = HttpContext.Session.GetString("Member");
            if (dataJson == null)
            {
                return RedirectToAction("Login", "Customer");
            }
            var customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));

            if (customer != null)
            {
                var orderBooks = _context.OrderBooks
                                         .Where(x => x.CustomerID == customer.CustomerID)
                                         .Include(p => p.TransactStatus)
                                         .Include(p => p.Payment)
                                         .ToList();

                ViewBag.orderBooks = orderBooks;
                return View(customer);
            }
            return View("Home");
        }
        //thông tin chi tiết đơn hàng
        public IActionResult orderDetail(int id)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("Member"));

            if (customer != null)
            {
                var orderDetails = _context.OrderDetails
                                         .Where(x => x.OrderId == id)
                                         .Include(p => p.Product)
                                         .Include(p => p.OrderBook)
                                         .ToList();
                var orderBook = _context.OrderBooks.Include(x=>x.TransactStatus).FirstOrDefault(x => x.OrderId == id);
                ViewBag.totalMoney = orderBook.TotalMoney;
                ViewBag.Status = orderBook.TransactStatus.Status;
                var orders = _context.OrderBooks.Where(x => x.CustomerID == customer.CustomerID).ToList();
                ViewBag.orders = orders;
                return View(orderDetails);
            }
            return View("Home");
        }
    }
}
