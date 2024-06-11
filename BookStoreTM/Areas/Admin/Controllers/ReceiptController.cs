using BookStoreTM.Models.EF;
using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ReceiptController : Controller
    {
        private readonly AppDbContext _db;

        public ReceiptController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

            var item = _db.Receipts.OrderByDescending(x => x.ReceiptId).ToPagedList(pageIndex, pageSize);
            if (!string.IsNullOrEmpty(name))
            {
                item = _db.Receipts.Where(x => x.Code.Contains(name)).ToPagedList(pageIndex, pageSize);
            }
            ViewBag.keyword = name;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _db.Receipts
                .Include(p => p.ReceiptDetails)
                .FirstOrDefaultAsync(m => m.ReceiptId == id);
            var receiptDetail = _db.ReceiptDetails.Where(x => x.ReceiptId == id).ToList();
            ViewBag.receiptDetail = receiptDetail;
            if (receipt == null)
            {
                return NotFound();
            }
            return View(receipt);
        }

        public IActionResult Create()
        {
            var model = new ReceiptCreateViewModel
            {
                ReceiptDetails = new List<ReceiptDetails> { new ReceiptDetails() } // Khởi tạo với một mục trống
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReceiptCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //model.Receipt.TotalMoney = model.ReceiptDetails.Sum(d => d.Price * d.Quatity);
                decimal total = 0;
                foreach (var item in model.ReceiptDetails)
                {
                    total += item.Price * item.Quatity;
                }
                model.Receipt.TotalMoney = total;
                //tạo mã 
                var strCode = "DH";
                string times = DateTime.Now.ToString("yyyy-MM-dd.HH-mm");
                strCode += "." + times;
                model.Receipt.Code = strCode;
                model.Receipt.CreatedDate = DateTime.Now;
                _db.Receipts.Add(model.Receipt);
                _db.SaveChanges();

                foreach (var detail in model.ReceiptDetails)
                {
                    detail.ReceiptId = model.Receipt.ReceiptId;
                    _db.ReceiptDetails.Add(detail);
                }
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _db.Receipts.Find(id);
            if (item != null)
            {
                var receipts = _db.ReceiptDetails.Where(x => x.ReceiptId == id).ToList();
                if (receipts.Count() > 0)
                {
                    return Json(new { success = false });
                }

                _db.Receipts.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
