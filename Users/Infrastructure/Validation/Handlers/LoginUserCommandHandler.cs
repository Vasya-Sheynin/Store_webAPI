using Infrastructure.Auth;
using MediatR;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.CommandHandlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAuthentication authentication;

        public LoginUserCommandHandler(IAuthentication auth)
        {
            authentication = auth;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var token = await authentication.LoginAsync(request.userLoginDto);

            return token;
        }
    }
}
