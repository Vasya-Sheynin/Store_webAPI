using CommonModules.Domain.Interfaces;
using Products.Application.ServiceInterfaces;
using Products.Application.Services;
using Products.Infrastructure.Persistence.Repositories;
using Products.Infrastructure.Auth.Extensions;
using Products.Infrastructure.Persistence.Extensions;
using Products.Application.Validation.Extensions;
using Hellang.Middleware.ProblemDetails;
using Products.Application.Extensions;

namespace Products.ProductsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerAuth();

            builder.Services.AddAuth(builder.Configuration);

            builder.Services.AddPersistence(builder.Configuration);

            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();

            builder.Services.AddValidation();

            builder.Services.AddExceptionHandling(builder.Environment);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
