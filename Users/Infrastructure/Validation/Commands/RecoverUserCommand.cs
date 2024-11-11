using MediatR;

namespace Users.Infrastructure.Validation.Commands
{
    public record RecoverUserCommand(string token, string password) : IRequest<string>;
}
