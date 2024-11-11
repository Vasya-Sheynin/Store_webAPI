using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Application.Validation.Behavior;

namespace Users.Application.Validation.Extensions
{
    public static class IServiceCollectionValidationExtension
    {
        public static void AddApplicationValidation(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ApplicationValidationBehavior<,>));
        }
    }
}
