using FluentValidation;
using System.Text.RegularExpressions;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.Validators
{
    public class SendRecoveryEmailValidator : AbstractValidator<SendRecoveryEmailCommand>
    {
        public SendRecoveryEmailValidator()
        {
            RuleFor(u => u.userRecoveryDto.Name)
                .Custom((name, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(name))
                    {
                        context.AddFailure("Name must contain only letters and numbers.");
                    }
                });

            RuleFor(u => u.userRecoveryDto.NewPassword)
                .Custom((password, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(password))
                    {
                        context.AddFailure("Password must contain only letters and numbers.");
                    }
                });

            RuleFor(u => u.userRecoveryDto.Email)
                .Custom((email, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$");
                    if (!regex.IsMatch(email))
                    {
                        context.AddFailure("Invalid email format.");
                    }
                });
        }
    }
}
