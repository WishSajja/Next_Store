using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;
using Next_Store.Models;

namespace Next_Store.Controllers
{
    
    public class PagesController : Controller
    {

        private readonly Next_StoreDbContext _context;
       
        public PagesController(Next_StoreDbContext context)
        {
            _context = context;
          
        }
        // Get / or /slug
        public async Task <IActionResult> Page(string slug)
        {
            if(string.IsNullOrEmpty(slug))
            {
               return View(await _context.Pages.Where(m=>m.Slug=="home").FirstOrDefaultAsync());      
            }
            Page? page = await _context.Pages.Where(m => m.Slug == slug).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}
