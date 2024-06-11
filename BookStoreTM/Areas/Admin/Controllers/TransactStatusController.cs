using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TransactStatusController : Controller
    {
        private readonly AppDbContext _context;

        public TransactStatusController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TransactStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.TransactStatus.ToListAsync());
        }

        // GET: Admin/TransactStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactStatus = await _context.TransactStatus
                .FirstOrDefaultAsync(m => m.TransactStatusID == id);
            if (transactStatus == null)
            {
                return NotFound();
            }

            return View(transactStatus);
        }

        // GET: Admin/TransactStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TransactStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactStatusID,Status")] TransactStatus transactStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactStatus);
        }

        // GET: Admin/TransactStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactStatus = await _context.TransactStatus.FindAsync(id);
            if (transactStatus == null)
            {
                return NotFound();
            }
            return View(transactStatus);
        }

        // POST: Admin/TransactStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactStatusID,Status")] TransactStatus transactStatus)
        {
            if (id != transactStatus.TransactStatusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactStatusExists(transactStatus.TransactStatusID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transactStatus);
        }
        private bool TransactStatusExists(int id)
        {
            return _context.TransactStatus.Any(e => e.TransactStatusID == id);
        }

        //xoá
        public IActionResult Delete(int id)
        {
            var item = _context.TransactStatus.Find(id);
            if (item != null)
            {
                var publishers = _context.TransactStatus.Include(x => x.OrderBook).FirstOrDefault(x => x.TransactStatusID == id);
                if (publishers != null && publishers.OrderBook.Any())
                {
                    return Json(new { success = false });
                }
                _context.TransactStatus.Remove(item);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
