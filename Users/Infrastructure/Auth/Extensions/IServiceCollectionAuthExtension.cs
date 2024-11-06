using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Infrastructure.Auth.Extensions
{
    public static class IServiceCollectionAuthExtension
    {
        public static void AddAuth(this IServiceCollection services, IConfiguration config)
        {
             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                Console.WriteLine(config.GetSection("Jwt")["Audience"]);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetSection("Jwt")["Issuer"],
                    ValidAudience = config.GetSection("Jwt")["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt")["Key"]))
                };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    Console.WriteLine($"Token: {token}");
                    Console.WriteLine($"ValidIssuer: {config.GetSection("Jwt:Issuer").Get<IEnumerable<string>>()}");
                    Console.WriteLine($"ValidAudience: {config.GetSection("Jwt:Audience").Get<IEnumerable<string>>()}");
                    Console.WriteLine($"IssuerSigningKey: {config.GetSection("Jwt:Key").Get<IEnumerable<string>>()}");
                    // Log the failure message
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    // Log token validation success
                    Console.WriteLine("Token validated successfully.");
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    // Log challenge details
                    Console.WriteLine($"Challenge: {context.ErrorDescription}");
                    return Task.CompletedTask;
                }
            };
            });

        }
    }
}
