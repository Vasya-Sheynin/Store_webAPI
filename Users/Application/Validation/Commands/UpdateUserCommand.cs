using MediatR;

namespace Users.Application.Validation.Commands
{
    public record UpdateUserCommand(Guid id, UpdateUserDto updateUserDto) : IRequest;
}
