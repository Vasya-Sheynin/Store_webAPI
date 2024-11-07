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
    public class FilterValidator : AbstractValidator<ValidateFilterCommand>
    {
        public FilterValidator()
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
