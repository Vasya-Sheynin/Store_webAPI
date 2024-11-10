using CommonModules.Domain.Entities;
using CommonModules.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Users.UsersApi;

namespace UsersTests.Setup
{
    internal class UsersWebAppFactory : WebApplicationFactory<Program>
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
                    new User(Guid.NewGuid(), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard),
                    new User(Guid.NewGuid(), "user2", "user2@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password2"), SecurityRoles.Admin)
                );
                dbContext.SaveChanges();
            });
        }
    }
}
