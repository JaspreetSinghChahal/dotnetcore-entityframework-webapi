using System.Net;
using System.Net.Mail;

namespace Autobot.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            string from = _emailConfig.From;
            string fromName = _emailConfig.FromName;
            string to = message.To;
            string smtpUsername = _emailConfig.SmtpUserName;
            string smtpPassword = _emailConfig.Password;
            string host = _emailConfig.Host;
            int port = _emailConfig.Port;

            // The subject line of the email
            string subject = message.Subject;

            // The body of the email
            string body = message.Content;

            // Create and build a new MailMessage object
            MailMessage mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(from, fromName)
            };
            mailMessage.To.Add(new MailAddress(to));
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            using (var client = new SmtpClient(host, port))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(smtpUsername, smtpPassword);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                client.Send(mailMessage);
            }
        }
    }
}