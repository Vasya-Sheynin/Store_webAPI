using MediatR;

namespace Users.Application.Validation.Commands
{
    public record CreateUserCommand(CreateUserDto createUserDto) : IRequest<UserDto>;
}
