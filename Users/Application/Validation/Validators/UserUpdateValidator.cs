using FluentValidation;
using System.Text.RegularExpressions;
using Users.Application.Validation.Commands;

namespace Users.Application.Validation.Validators
{
    public class UserUpdateValidator : AbstractValidator<UpdateUserCommand>
    {
        public UserUpdateValidator()
        {
            RuleFor(u => u.updateUserDto.Name)
                .Custom((name, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(name))
                    {
                        context.AddFailure("Name must contain only letters and numbers.");
                    }
                });


            RuleFor(u => u.updateUserDto.Password)
                .Custom((password, context) =>
                {
                    var regex = new Regex("^[a-zA-Z0-9]+$");
                    if (!regex.IsMatch(password))
                    {
                        context.AddFailure("Password must contain only letters and numbers.");
                    }
                });

            RuleFor(u => u.updateUserDto.Email)
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
