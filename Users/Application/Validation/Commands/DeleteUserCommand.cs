using MediatR;

namespace Users.Application.Validation.Commands
{
    public record DeleteUserCommand(Guid id) : IRequest;
}
