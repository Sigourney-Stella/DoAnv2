using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using X.PagedList;

namespace BookStoreTM.Controllers
{
    public class NewsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<NewsController> _logger;

        public NewsController(AppDbContext db, ILogger<NewsController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IActionResult Index(int page = 1)
        {
            int limit = 2;
            var news = _db.News.Where(x => x.IsActicve).OrderBy(x => x.CreatedDate).ToPagedList(page, limit);
            return View(news);
        }

        public IActionResult Detail(int? id)
        {
            var news = _db.News.ToList();
            var item = _db.News.Find(id);

            var newBlog = _db.News.Where(x => x.IsActicve).Take(3).ToList();
            ViewBag.newBlog = newBlog;

            return View(item);
        }
    }
}
