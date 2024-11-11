namespace Users.Infrastructure.Email
{
    public interface IEmailSender
    {
        public Task SendEmail(Message message);
    }
}
