using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly Next_StoreDbContext _context;
        public CategoriesController(Next_StoreDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()=> View(await _context.Categories.OrderBy(m => m.Sorting).ToListAsync());
        // GET /Admin/Categories/Index/id
        public async Task<IActionResult> Details(int id)
        {
            CategoryModel? category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // Create new Category(GET)
        public IActionResult Create() => View();
        // Create new page(POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "_");
            category.Sorting = 100;
            var slug = await _context.Categories.FirstOrDefaultAsync(d => d.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "The Category is Already in use");
                return View(category);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category Created!!";
            return RedirectToAction("Index");
            }
            return View("Create", category);
        }
        // GET Edit Details
        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await _context.Categories.FirstAsync(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "_");

            var slug = await _context.Products.Where(d => d.Id != category.Id).FirstOrDefaultAsync(d => d.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Title Already in use");
                return View("Create", category);
            }
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category Upadated!!";
            return RedirectToAction("Edit", new { id });
            }
            return View("Edit", category);
        }
        // GET Admin/categories/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel category = await _context.Categories.FirstAsync(p => p.Id == id);
            if (category == null)
            {
                TempData["Error"] = "Category does not exist!!";
            }
            else
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Category Deleted!!";
            }
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach (var categoryId in id)
            {
                CategoryModel category = await _context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                _context.Update(category);
                await _context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }


    }
}
