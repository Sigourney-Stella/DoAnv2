using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _db;

        public ProductCategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 5; 
            var items = _db.ProductCategories.OrderByDescending(x => x.ProductCategoryId).ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                items = _db.ProductCategories.Where(x => x.Name.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.keyword = name;
            return View(items);
        }

        //thêm danh mục sản phẩm
        [HttpGet]
        public IActionResult ThemDMSanPham()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemDMSanPham(ProductCategory SanPham)
        {
            if (ModelState.IsValid)
            {
                SanPham.CreatedDate = DateTime.Now;
                SanPham.Alias = BookStoreTM.Common.Filter.FilterChar(SanPham.Name);

                _db.ProductCategories.Add(SanPham);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(SanPham);
        }

        //sửa 
        [HttpGet]
        public IActionResult SuaDMSanPham(int MaSanPham)
        {
            var sanPham = _db.ProductCategories.Find(MaSanPham);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaDMSanPham(ProductCategory MaLoai)
        {
            if (ModelState.IsValid)
            {
                MaLoai.UpdatedDate = DateTime.Now;
                MaLoai.Alias = BookStoreTM.Common.Filter.FilterChar(MaLoai.Name);

                _db.Update(MaLoai);
                _db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(MaLoai);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.ProductCategories.Find(id);
            if (item != null)
            {
                var category = _db.ProductCategories.Include(pc => pc.Products).FirstOrDefault(pc => pc.ProductCategoryId == id);

                if (category != null && category.Products.Any())
                {
                    return Json(new { success = false });
                }
                _db.ProductCategories.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        //[HttpPost]
        //public ActionResult DeleteAll(string ids)
        //{
        //    if (!string.IsNullOrEmpty(ids))
        //    {
        //        var items = ids.Split(',');
        //        if (items != null && items.Any())
        //        {
        //            foreach (var item in items)
        //            {
        //                var categoryId = Convert.ToInt32(item);
        //                var category = _db.ProductCategories.Include(pc => pc.Products).FirstOrDefault(pc => pc.ProductCategoryId == categoryId);

        //                if (category != null && category.Products.Any())
        //                {
        //                    return Json(new { success = false, message = "Danh mục " + category.Name + " có sản phẩm và không thể bị xóa." });
        //                }
        //                _db.ProductCategories.Remove(category);
        //                _db.SaveChanges();
        //            }
        //        }
        //        return Json(new { success = true });
        //    }
        //    return Json(new { success = false });
        //}
    }
}
