using FluentValidation;
using System.Text.RegularExpressions;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.Validators
{
    public class RecoverUserValidator : AbstractValidator<RecoverUserCommand>
    {
        public RecoverUserValidator()
        {
            RuleFor(u => u.password)
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
