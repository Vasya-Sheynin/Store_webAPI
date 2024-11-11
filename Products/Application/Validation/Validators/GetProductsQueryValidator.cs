using FluentValidation;
using Products.Application.Validation.Queries;

namespace Products.Application.Validation.Validators
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(f => f.filter.MaxPrice)
                .Custom((price, context) =>
                {
                    if (price < 0)
                    {
                        context.AddFailure("Price cannot be negative.");
                    }
                });

            RuleFor(f => f.filter.MinPrice)
                .Custom((price, context) =>
                {
                    if (price < 0)
                    {
                        context.AddFailure("Price cannot be negative.");
                    }
                });
        }
    }
}
