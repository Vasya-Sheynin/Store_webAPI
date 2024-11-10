using CommonModules.Domain.Entities;
using CommonModules.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Products.ProductsApi;

namespace ProductsTests.Setup
{
    internal class ProductsWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureDeleted();

                dbContext.AddRange(
                    new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard),
                    new Product(Guid.Parse("50fa81d9-b0a7-4653-a880-9411e28d5ff9"), "Tv", "", 12.2, Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), DateTime.Now)
                );
                dbContext.SaveChanges();
            });
        }
    }
}
