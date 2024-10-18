using Microsoft.EntityFrameworkCore;
using Store_webAPI.Data.Entities;

namespace Store_webAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
