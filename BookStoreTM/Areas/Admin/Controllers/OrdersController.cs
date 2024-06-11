using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int? statusId, int? page)
        {
            IPagedList<OrderBook> orders = _db.OrderBooks.Include(p => p.Customer).Include(p => p.TransactStatus).OrderByDescending(p => p.OrderId).ToPagedList();
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            
            if (!string.IsNullOrEmpty(name))
            {
                orders = _db.OrderBooks.Where(x => x.CodeOrder.Contains(name)).ToPagedList(pageIndex, pageSize);
            }
            //lọc
            statusId = statusId ?? 0;
            var transacStatus = _db.TransactStatus.ToList();

            transacStatus.Insert(0, new TransactStatus { TransactStatusID = 0, Status = "chọn danh mục" });

            ViewBag.statusId = new SelectList(transacStatus, "TransactStatusID", "Status", statusId);

            if (statusId > 0)
            {
                orders = orders.Where(x => x.TransactStatusID == statusId).OrderByDescending(p => p.OrderId).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                orders = orders.ToPagedList(pageIndex, pageSize);
            }
            //phân trang 
            ViewBag.transactId = statusId;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;

            //phương thức thanh toán
            var dataStatus = _db.TransactStatus.ToList();
            ViewData["IdStatus"] = new SelectList(dataStatus, "TransactStatusID", "Status");
            ViewBag.keyword = name;
            return View(orders);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var item = _db.OrderBooks.Find(id);
            var tranStatus = _db.OrderBooks.Include(x => x.TransactStatus).Where(x => x.OrderId == id).ToList();
            ViewBag.tranStatus = tranStatus;

            var productDetail = _db.OrderDetails.Where(x => x.OrderId == id).Include(p => p.Product).ToList();
            ViewBag.productDetail = productDetail;

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, int IdStatus)
        {
            var order = await _db.OrderBooks.FindAsync(orderId);
            if (order != null)
            {
                if (IdStatus == 3) // nếu là trạng thái 
                {
                    var orderDetail = _db.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                    foreach (var detail in orderDetail)
                    {
                        var product = _db.Products.Find(detail.ProductId);
                        if (product != null)
                        {
                            product.Quatity += detail.Quatity;
                        }
                    }
                }
                order.TransactStatusID = IdStatus;
                _db.Update(order);
                await _db.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

