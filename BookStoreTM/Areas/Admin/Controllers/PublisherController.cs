using BookStoreTM.Common;
using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PublisherController : Controller
    {
        private readonly AppDbContext _db;

        public PublisherController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 10;
            var items = _db.Publishers.OrderByDescending(x => x.PublisherId).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                items = _db.Publishers.Where(x => x.PublisherName.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
            return View(items);
        }

        //thêm mới danh mục
        [HttpGet]
        public IActionResult ThemNXB()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNXB(Publisher model)
        {
            if (ModelState.IsValid)
            {
                //DanhMuc.UpdatedDate = DateTime.Now;
                _db.Publishers.Add(model);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(model);
        }

        //sửa 
        [HttpGet]
        public IActionResult SuaNXB(int model)
        {
            var loai = _db.Publishers.Find(model);
            return View(loai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaNXB(Publisher loai)
        {
            if (ModelState.IsValid)
            {
                _db.Update(loai);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(loai);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.Publishers.Find(id);
            if (item != null)
            {
                var publishers = _db.Publishers.Include(x => x.Products).FirstOrDefault(x => x.PublisherId == id);
                if (publishers != null && publishers.Products.Any())
                {
                    return Json(new { success = false });
                }
                _db.Publishers.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
