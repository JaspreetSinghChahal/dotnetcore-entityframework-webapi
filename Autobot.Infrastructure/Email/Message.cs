using Microsoft.AspNetCore.Http;

namespace Autobot.Infrastructure.Email
{
    public class Message
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public IFormFileCollection Attachments { get; set; }

        public Message(string to, string subject, string content, IFormFileCollection attachments = null)
        {
            To = to;
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
