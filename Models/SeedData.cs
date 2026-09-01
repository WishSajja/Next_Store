using Microsoft.EntityFrameworkCore;
using Next_Store.Infrastructure;

namespace Next_Store.Models
{
    public class SeedData
    {
        private readonly Next_StoreDbContext _context;
        public SeedData(Next_StoreDbContext context)
        {
            _context = context; 
        }

        public void Initialize()
        {
               if(_context.Pages.Any())
               {
                    return;
               }
                _context.Pages.AddRange(
                    new Page 
                    {
                        Title="Home",
                        Slug="home",
                        Content="home page",
                        Sorting=0

                    },
                     new Page
                     {
                         Title = "About Us",
                         Slug = "about-us",
                         Content = "about us page",
                         Sorting = 1

                     },
                      new Page
                      {
                          Title = "Services",
                          Slug = "services",
                          Content = "services page",
                          Sorting = 2

                      },
                       new Page
                       {
                           Title = "Contact",
                           Slug = "contact",
                           Content = "contact page",
                           Sorting = 3

                       }
                    );
                _context.SaveChanges();
            
        }
    }
}
