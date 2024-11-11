using FluentValidation;
using System.Text.RegularExpressions;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.Validators
{
    public class UserLoginValidator : AbstractValidator<LoginUserCommand>
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
