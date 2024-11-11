using MediatR;
using Users.Application;

namespace Users.Infrastructure.Validation.Commands
{
    public record RegisterUserCommand(UserRegisterDto userRegisterDto) : IRequest<string>;
}
