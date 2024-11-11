using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonModules.Persistence.Extensions
{
    public static class IServiceCollectionPersistenceExtension
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = config.GetConnectionString("DbConnection");
                options.UseSqlServer(connectionString);
            });
        }
    }
}
