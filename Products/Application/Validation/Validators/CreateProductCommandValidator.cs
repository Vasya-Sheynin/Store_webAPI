using FluentValidation;
using Products.Application.Validation.Commands;
using System.Text.RegularExpressions;

namespace Products.Application.Validation.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.createProductDto.Name)
                .Custom((name, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(name))
                    {
                        context.AddFailure("Name must contain only letters and numbers.");
                    }
                });

            RuleFor(p => p.createProductDto.Price)
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
