using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Infrastructure.Validation.Behavior;

namespace Users.Infrastructure.Validation.Extensions
{
    public static class IServiceCollectionValidationExtension
    {
        public static void AddInfrastructureValidation(this IServiceCollection services)
        {
            services.AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(InfrastructureValidationBehavior<,>));
        }
    }
}
