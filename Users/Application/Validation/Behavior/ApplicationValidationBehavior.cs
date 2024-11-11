using FluentValidation;
using MediatR;
using Users.Application.Exceptions;

namespace Users.Application.Validation.Behavior
{
    public class ApplicationValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ApplicationValidationBehavior(IEnumerable<IValidator<TRequest>> v)
        {
            validators = v;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var errors = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x is not null)
                .ToList();

            if (errors.Any())
            {
                throw new UserValidationException(errors);
            }

            return await next();
        }
    }

}
