using Infrastructure.Auth;
using MediatR;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAuthentication authentication;

        public RegisterUserCommandHandler(IAuthentication auth)
        {
            authentication = auth;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var token = await authentication.RegisterAsync(request.userRegisterDto);

            return token;
        }
    }
}
