namespace Autobot.Infrastructure.Email
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}