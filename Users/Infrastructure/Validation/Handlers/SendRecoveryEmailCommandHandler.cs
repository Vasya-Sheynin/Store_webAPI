using Infrastructure.Auth;
using MediatR;
using Users.Infrastructure.Email;
using Users.Infrastructure.Validation.Commands;

namespace Users.Infrastructure.Validation.Handlers
{
    public class SendRecoveryEmailCommandHandler : IRequestHandler<SendRecoveryEmailCommand>
    {
        private readonly IAuthentication authentication;
        private readonly IEmailSender emailSender;
        private readonly EmailConfig emailConfig;

        public SendRecoveryEmailCommandHandler(IAuthentication auth, IEmailSender sender, EmailConfig config)
        {
            authentication = auth;
            emailSender = sender;
            emailConfig = config;
        }

        public async Task Handle(SendRecoveryEmailCommand request, CancellationToken cancellationToken)
        {
            var recoveryToken = await authentication.GenerateRecoveryTokenAsync(request.userRecoveryDto);
            var emailHtmlBody = $"<a href='https://localhost:7225/store-api/auth/recovery?token={recoveryToken}&password={request.userRecoveryDto.NewPassword}'>Click here to reset your password</a>";
            var message = new Message(emailConfig.From, "Password recovery", emailHtmlBody);
            await emailSender.SendEmail(message);
        }
    }
}
