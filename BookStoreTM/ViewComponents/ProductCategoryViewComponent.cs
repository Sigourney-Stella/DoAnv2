using BookStoreTM.Models;
using BookStoreTM.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BookStoreTM.ViewComponents
{
    public class ProductCategoryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var product = await _context.ProductCategories.ToListAsync();
             
            var cartTotalQuantity = HttpContext.Session.GetInt32("CartTotalQuantity") ?? 0;
            ViewBag.CartTotalQuantity = cartTotalQuantity;

            var dataLogin = _context.Customers.ToListAsync();
            ViewBag.Customer = dataLogin;

            //tìm kiếm 
            var products = _context.Products.Include(p => p.ProductCategory).ToListAsync();
            if (!string.IsNullOrEmpty(name))
            {
                //tìm kiếm theo tên sách hoặc tên nhà xuất bản
                products = _context.Products.Where(x => x.ProductName.Contains(name) || x.Author.Contains(name)).ToListAsync();
            }
            ViewBag.keyword = name;
            return View(product);
        }
    }
}
