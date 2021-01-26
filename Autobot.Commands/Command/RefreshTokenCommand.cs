using Autobot.Infrastructure.Auth.Model;
using MediatR;

namespace Autobot.Commands.Command
{
    public class RefreshTokenCommand : IRequest<Token>
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
