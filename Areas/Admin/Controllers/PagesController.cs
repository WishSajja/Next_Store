using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly Next_StoreDbContext _context;
        public PagesController(Next_StoreDbContext context)
        {
            _context = context;
        }
        // GET /Admin/Page/Index
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> page = _context.Pages.OrderBy(p=>p.Sorting);
            List<Page> pageList = await page.ToListAsync();
            return View(pageList);
        }
        // GET /Admin/Page/Index/id
        public async Task<IActionResult> Details(int id)
        {
            Page page= await _context.Pages.FirstOrDefaultAsync(p=>p.Id==id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
        // Create new page(GET)
             public IActionResult Create() => View("CreatePage");
        // Create new page(POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "_");
                page.Sorting = 100;
                var slug = await _context.Pages.FirstOrDefaultAsync(d => d.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The Page Already in use");
                    return View("CreatePage", page);
                }
                _context.Pages.Add(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Page Created!!";
                return RedirectToAction("Index");
            }
            return View("CreatePage", page);
        }
        // Edit Details
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await _context.Pages.FirstAsync(p => p.Id == id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "_");
              
                var slug = await _context.Pages.Where(d=>d.Id!=page.Id).FirstOrDefaultAsync(d => d.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Title Already in use");
                    return View("CreatePage", page);
                }
                _context.Pages.Update(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Page Upadated!!";
                return RedirectToAction("Edit", new {id =page.Id });
            }
            return View("CreatePage", page);
        }
        // GET Admin/Page/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await _context.Pages.FirstAsync(p => p.Id == id);
            if (page == null)
            {
                TempData["Error"] = "Page does not exist!!";
            }
            else
            {
                _context.Pages.Remove(page);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Page Deleted!!";
            }
            return RedirectToAction("Index");
            
        }
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach (var pageId in id)
            {
                Page page = await _context.Pages.FindAsync(pageId);
                page.Sorting = count;
                _context.Update(page);
                await _context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
