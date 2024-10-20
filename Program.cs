
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_webAPI.Data;

namespace Store_webAPI
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Database", "StoreDatabase.mdf");
                var connectionString = string.Format("{0} AttachDbFilename={1};",
                    builder.Configuration.GetConnectionString("DbConnection"), databasePath);
                options.UseSqlServer(connectionString);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
