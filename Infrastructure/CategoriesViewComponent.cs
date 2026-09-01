using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Next_Store.Models;

namespace Next_Store.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly Next_StoreDbContext _context;

        public CategoriesViewComponent(Next_StoreDbContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }
        public Task<List<CategoryModel>> GetCategoriesAsync()
        {
            return _context.Categories.OrderBy(m => m.Sorting).ToListAsync();
        }
    }
}
