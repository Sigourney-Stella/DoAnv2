using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeAdminController : Controller
    {
        private readonly AppDbContext _db;

        public HomeAdminController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var soLuongHD = _db.OrderBooks.Count();
            ViewBag.soLuongHD = soLuongHD;
            var soLuongSP = _db.Products.Count();
            ViewBag.soLuongSP = soLuongSP;
            var soLuongDonCho = _db.OrderBooks.Where(x => x.TransactStatusID == 1).Count();
            ViewBag.soLuongDonCho = soLuongDonCho;
            var soLuongDonHT = _db.OrderBooks.Where(x => x.TransactStatusID == 2).Count();
            ViewBag.soLuongDonHT = soLuongDonHT;
            var soLuongDonHuy = _db.OrderBooks.Where(x => x.TransactStatusID == 3).Count();
            ViewBag.soLuongDonHuy = soLuongDonHuy;
            var baiViet = _db.News.Count();
            ViewBag.baiViet = baiViet;

            return View();
        }

        [HttpGet]
        public IActionResult GetThongKe(string fromDate, string toDate)
         {
            //DateTime startDates = DateTime.Now.AddDays(-7);
            //DateTime endDates = DateTime.Now;

            var query = from t in _db.TransactStatus
                        join o in _db.OrderBooks
                        on t.TransactStatusID equals o.TransactStatusID
                        join od in _db.OrderDetails
                        on o.OrderId equals od.OrderId
                        join p in _db.Products
                        on od.ProductId equals p.ProductId
                        where o.TransactStatusID == 2 /*& o.OrderDate > DateTime.Now.AddDays(-7)*/
                        select new
                        {
                            TransactStatusID = t.TransactStatusID,
                            CreatedDate = o.OrderDate,
                            Quantity = od.Quatity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.Parse(fromDate);
                query = query.Where(x => x.CreatedDate >= startDate);
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                //DateTime endDate = DateTime.Parse(toDate);
                DateTime endDate = DateTime.Parse(toDate).AddDays(1);
                query = query.Where(x => x.CreatedDate <= endDate);
            }

            var result = query.GroupBy(x => x.CreatedDate.Date).Select(x => new
            {
                Date = x.Key,
                TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
            }).Select(x => new
            {
                Date = x.Date,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            }).ToList();

            return Json(new { Data = result });
        }
    }
}
