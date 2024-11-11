using MediatR;
using Users.Application;

namespace Users.Infrastructure.Validation.Commands
{
    public record LoginUserCommand(UserLoginDto userLoginDto) : IRequest<string>;
}
