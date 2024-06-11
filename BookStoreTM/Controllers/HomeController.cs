using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookStoreTM.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _db.Products.Include(c => c.ProductCategory).Where(x => x.IsHome && x.IsActicve).Take(12).ToList();

            var productSale =  _db.Products.Include(c => c.ProductCategory).Where(x => x.IsHome && x.IsActicve && x.IsSale).Take(12).ToList();
            ViewBag.productSale = productSale;

            var news = _db.News.Where(x => x.IsActicve).Take(3).ToList();
            ViewBag.news = news;

            return View(products);
        }
        public IActionResult LienHe()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
