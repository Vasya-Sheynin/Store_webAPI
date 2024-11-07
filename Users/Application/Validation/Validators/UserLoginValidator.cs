using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Users.Application.Validation.Commands;

namespace Users.Application.Validation.Validators
{
    public class UserLoginValidator : AbstractValidator<ValidateUserLoginCommand>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.userLoginDto.Name)
                .Custom((name, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(name))
                    {
                        context.AddFailure("Name must contain only letters and numbers.");
                    }
                });

            RuleFor(u => u.userLoginDto.Password)
                .Custom((password, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(password))
                    {
                        context.AddFailure("Password must contain only letters and numbers.");
                    }
                });
        }
    }
}
