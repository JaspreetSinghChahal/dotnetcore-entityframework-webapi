namespace Autobot.Infrastructure.Email
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string FromName { get; set; }
        public string Host { get; set; }
        public string SmtpUserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}
