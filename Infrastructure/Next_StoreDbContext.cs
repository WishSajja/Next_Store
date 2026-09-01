using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Next_Store.Models;

namespace Next_Store.Infrastructure
{
    public class Next_StoreDbContext : IdentityDbContext<AppUser>
    {
        public Next_StoreDbContext(DbContextOptions<Next_StoreDbContext> options)
            :base(options)
        {

        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductsModel> Products { get; set; }
        
    }
}
