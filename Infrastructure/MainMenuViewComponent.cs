using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Models;

namespace Next_Store.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly Next_StoreDbContext _context;

        public MainMenuViewComponent(Next_StoreDbContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }
        public Task<List<Page>> GetPagesAsync()
        {
            return _context.Pages.OrderBy(m => m.Sorting).ToListAsync();
        }
    }
}
