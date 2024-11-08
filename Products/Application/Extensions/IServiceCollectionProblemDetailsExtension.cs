using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.Application.Exceptions;

namespace Products.Application.Extensions
{
    public static class IServiceCollectionProblemDetailsExtension
    {
        public static void AddExceptionHandling(this IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddProblemDetails(options =>
            {
                options.ExceptionDetailsPropertyName = "Exception details";
                options.IncludeExceptionDetails = (context, exception) => environment.IsDevelopment() || environment.IsStaging();

                options.Map<ProductNotFoundException>(exception => new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Detail = exception.Detail,
                    Instance = exception.Instance,
                    Title = exception.Title,
                    Type = exception.Type
                });

                options.Map<NoAccessException>(exception => new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Detail = exception.Detail,
                    Instance = exception.Instance,
                    Title = exception.Title,
                    Type = exception.Type
                });

                options.Map<ProductValidationException>(exception => new ProblemDetails
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
