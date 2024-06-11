using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _db;

        public CustomerController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string name, int page = 1)
        {
            int limit = 10;
            var item = _db.Customers.ToPagedList(page, limit);
            if (!string.IsNullOrEmpty(name))
            {
                //tìm kiếm theo tên sách hoặc tên nhà xuất bản
                item = _db.Customers.Where(x => x.Fullname.Contains(name)).ToPagedList(page, limit);
            }
            ViewBag.PageSize = limit;
            ViewBag.Page = page;
            ViewBag.keyword = name;
            return View(item);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }
    }
}
