namespace Autobot.Infrastructure.Auth.Model
{
    public sealed class Token
    {
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public int ValidFor { get; set; }
        public string RefreshToken { get; set; }
    }
}
