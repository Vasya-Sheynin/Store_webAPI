using CommonModules.Domain.Interfaces;
using CommonModules.Persistence.Extensions;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Auth;
using Users.Application.Extensions;
using Users.Application.ServiceInterfaces;
using Users.Application.Services;
using Users.Application.Validation.Extensions;
using Users.Infrastructure.Auth;
using Users.Infrastructure.Auth.Extensions;
using Users.Infrastructure.Email;
using Users.Infrastructure.Persistence.Repositories;
using Users.Infrastructure.Validation.Extensions;

namespace Users.UsersApi
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
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IAuthentication, Authentication>();

            builder.Services.AddApplicationValidation();
            builder.Services.AddInfrastructureValidation();

            builder.Services.AddExceptionHandling(builder.Environment);


            var emailConfig = builder.Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfig>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddScoped<IEmailSender, EmailSender>();

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
