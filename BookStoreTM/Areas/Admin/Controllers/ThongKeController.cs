using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ThongKeController : Controller
    {
        private readonly AppDbContext _db;

        public ThongKeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int statusId)
        {
            if(statusId == 0)
            {

            }
            var soLuongHD = _db.OrderBooks.Count();
            ViewBag.soLuongHD = soLuongHD;
            var soLuongSP = _db.Products.Count();
            ViewBag.soLuongSP = soLuongSP;
            var soLuongDonHT = _db.OrderBooks.Where(x => x.TransactStatusID == 2).Count();
            ViewBag.soLuongDonHT = soLuongDonHT;
            var soLuongDonHuy = _db.OrderBooks.Where(x => x.TransactStatusID == 3).Count();
            ViewBag.soLuongDonHuy = soLuongDonHuy;

            return View();
        }
        [HttpGet]
        public IActionResult GetThongKe(string fromDate, string toDate)
        {
            var query = from o in _db.OrderBooks
                        join od in _db.OrderDetails
                        on o.OrderId equals od.OrderId
                        join p in _db.Products
                        on od.ProductId equals p.ProductId
                        select new
                        {
                            CreatedDate = o.OrderDate,
                            Quantity = od.Quatity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            return View();
        }
    }
}
