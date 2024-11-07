using FluentValidation;
using Products.Application.Validation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Products.Application.Validation.Validators
{
    public class ProductInsertValidator : AbstractValidator<ValidateProductInsertCommand>
    {
        public ProductInsertValidator()
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
