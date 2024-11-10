using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Users.Application.Exceptions;

namespace Users.Application.Extensions
{
    public static class IServiceCollectionProblemDetailsExtension
    {
        public static void AddExceptionHandling(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddProblemDetails(options =>
            {
                options.ExceptionDetailsPropertyName = "Exception details";
                options.IncludeExceptionDetails = (context, exception) => environment.IsDevelopment() || environment.IsStaging();

                options.Map<UserNotFoundException>(exception => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Detail = exception.Detail,
                    Instance = exception.Instance,
                    Title = exception.Title,
                    Type = exception.Type
                });

                options.Map<UserAlreadyExistsException>(exception => new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Detail = exception.Detail,
                    Instance = exception.Instance,
                    Title = exception.Title,
                    Type = exception.Type
                });

                options.Map<UserValidationException>(exception => new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation exception",
                    Type = exception.Type,
                    Detail = exception.Message,
                    Instance = exception.Source
                });
            });
        }
    }
}
