using Infrastructure.Auth;
using MediatR;
using System.Security.Claims;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.CommandHandlers
{
    public class RecoverUserCommandHandler : IRequestHandler<RecoverUserCommand, string>
    {
        private readonly IAuthentication authentication;

        public RecoverUserCommandHandler(IAuthentication auth)
        {
            authentication = auth;
        }

        public async Task<string> Handle(RecoverUserCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(authentication.ParseToken(request.token).First(claim => claim.Type == ClaimTypes.Uri).Value);

            var loginToken = await authentication.RecoverAsync(userId, request.password);

            return loginToken;
        }
    }
}
