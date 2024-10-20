using Microsoft.EntityFrameworkCore;
using Store_webAPI.Data.Entities;

namespace Store_webAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
