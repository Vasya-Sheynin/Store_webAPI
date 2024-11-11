using MediatR;
using Users.Application;

namespace Users.Infrastructure.Validation.Commands
{
    public record SendRecoveryEmailCommand(UserRecoveryDto userRecoveryDto) : IRequest;
}
