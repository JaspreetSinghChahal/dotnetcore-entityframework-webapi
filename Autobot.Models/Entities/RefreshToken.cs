using System;

namespace Autobot.Models.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public string UserId { get; set; }
        public string RemoteIpAddress { get; set; }
    }
}
