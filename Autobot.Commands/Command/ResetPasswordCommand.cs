using MediatR;

namespace Autobot.Commands.Command
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
