using BookStoreTM.Models.Entities;
using BookStoreTM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using BookStoreTM.Common;
using System.Collections.Generic;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index(string name, int? page, int? categoryId)
        {
            IPagedList<Product> products = _db.Products.Include(p => p.ProductCategory).OrderByDescending(p => p.ProductId).ToPagedList();

            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            categoryId = categoryId ?? 0;
            //products = _db.Products.Include(p => p.ProductCategory).OrderByDescending(p => p.ProductId).ToPagedList(pageIndex, pageSize);

            var Categories = _db.ProductCategories.ToList();

            Categories.Insert(0, new ProductCategory { ProductCategoryId = 0, Name = "chọn danh mục" });

            ViewBag.CategoryId = new SelectList(Categories, "ProductCategoryId", "Name", categoryId);

            if (categoryId > 0)
            {
                products = products.Where(x => x.ProductCategoryId == categoryId).OrderByDescending(p => p.ProductId).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                products = products.ToPagedList(pageIndex, pageSize);
            }

            if (!string.IsNullOrEmpty(name))
            {
                products = _db.Products.Where(x => x.ProductName.Contains(name)).ToPagedList(pageIndex, pageSize);
            }
            ViewBag.keyword = name;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.cateId = categoryId;
            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Publisher)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            var productImg = _db.ProductImages.Where(x => x.ProductId == id).Include(p => p.Product).ToList();
            ViewBag.productImg = productImg;

            return View(product);
        }
        //ckeditor
        public IActionResult UploadImage(List<IFormFile> files)
        {
            var filepath = "";
            foreach (IFormFile photo in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, "product", photo.FileName);
                using (var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                filepath = "https://localhost:44388/" + "product/" + photo.FileName;
            }
            return Json(new { url = filepath });
        }

        //thêm mới
        [HttpGet]
        public IActionResult ThemSanPham()
        {
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPham(Product model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                    {
                        var file = files[0];
                        var FileName = file.FileName;
                        string[] tokens = FileName.Split('.');
                        var nameImg = "SanPham" + ConvertVietNamToEnglish.LocDau(model.ProductName) + "." + tokens[tokens.Length - 1];
                        string result = nameImg.Replace(" ", "");
                        // upload ảnh vào thư mục wwwroot\\images\\category
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagesAdmin\\products", result);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            model.Images = "/imagesAdmin/products/" + result; // gán tên ảnh cho thuộc tinh Image
                        }
                    }
                    model.CreatedDate = DateTime.Now;
                    if (string.IsNullOrEmpty(model.Alias))
                        model.Alias = BookStoreTM.Common.Filter.FilterChar(model.ProductName);
                    _db.Add(model);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message.ToString();
                ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
                ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
                return View(model);
            }
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
            return View(model);
        }

        //sửa 
        [HttpGet]
        public IActionResult SuaSanPham(int model)
        {
            var sanpham = _db.Products.Find(model);
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName", sanpham.ProductId);
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name", sanpham.ProductCategoryId);
            return View(sanpham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(int id, Product model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0].Length > 0) // kiểm tra xem tập có đc gửi từ file lên không 
                {
                    var file = files[0];
                    var FileName = file.FileName;
                    string[] tokens = FileName.Split('.');
                    var nameImg = "SanPham" + ConvertVietNamToEnglish.LocDau(model.ProductName) + "." + tokens[tokens.Length - 1];
                    string result = nameImg.Replace(" ", "");
                    // upload ảnh vào thư mục wwwroot\\images\\category
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\imagesAdmin\\products", result);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        model.Images = "/imagesAdmin/products/" + result; // gán tên ảnh cho thuộc tinh Image
                    }
                }
                model.UpdatedDate = DateTime.Now;
                model.Alias = BookStoreTM.Common.Filter.FilterChar(model.ProductName);

                _db.Update(model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Publisher = new SelectList(_db.Publishers.ToList(), "PublisherId", "PublisherName");
            ViewBag.ProductCategory = new SelectList(_db.ProductCategories.ToList(), "ProductCategoryId", "Name");
            return View(model);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                var products = _db.OrderDetails.Where(x => x.ProductId == id).ToList();
                if (products.Count() > 0)
                {
                    return Json(new { success = false });
                }

                //xoá ảnh
                var proImg = _db.ProductImages.Where(x => x.ProductId == id).ToList();
                if (proImg.Any())
                {
                    _db.RemoveRange(proImg);
                }

                _db.Products.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        //bật tắt tin tức
        public IActionResult IsActicve(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsActicve = !item.IsActicve;
                var entry = _db.Entry<Product>(item);
                entry.State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isActicve = item.IsActicve });
            }
            return Json(new { success = false });
        }
        public IActionResult IsHome(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                var entry = _db.Entry<Product>(item);
                entry.State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }
            return Json(new { success = false });
        }
        public IActionResult IsSale(int id)
        {
            var item = _db.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                var entry = _db.Entry<Product>(item);
                entry.State = EntityState.Modified;
                _db.SaveChanges();
                return Json(new { success = true, isSale = item.IsSale });
            }
            return Json(new { success = false });
        }

        //thêm nhiều hình ảnh
        public IActionResult DetailImage(int id)
        {
            var productImage = _db.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(productImage);
        }

    }
}
