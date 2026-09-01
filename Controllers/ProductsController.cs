using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly Next_StoreDbContext _context;

        public ProductsController(Next_StoreDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index(int p=1)
        {
            int itemsPerPage = 6;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = itemsPerPage;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / itemsPerPage);

            return View(await _context.Products.OrderByDescending(m => m.Id).Include(m => m.Category)
                 .Skip((p - 1) * itemsPerPage)
                 .Take(itemsPerPage)
                 .ToListAsync());
           


        }
        // Get /Product/Category (products based on their category)
        [Route("/Products/ProductByCategory/{categoryslug}/{p?}")]
        public async Task<IActionResult> ProductByCategory(string categoryslug, int p = 1)
        {
            CategoryModel category = await _context.Categories.Where(m => m.Slug == categoryslug).FirstOrDefaultAsync();
            
            if (category == null)
            {
               return RedirectToAction("Index");
            }
            ViewBag.CategoryName = category.Name;
            int itemsPerPage = 6;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = itemsPerPage;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / itemsPerPage);
            var products = await _context.Products.OrderByDescending(m => m.Id).Include(m => m.Category).Where(m => m.CategoryId == category.Id)
                 .Skip((p - 1) * itemsPerPage)
                 .Take(itemsPerPage)
                 .ToListAsync();

            return View(products);


        }
        
    }
}
