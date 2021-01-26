using MediatR;

namespace Autobot.Commands.Command
{
    public class UserPointResetCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public string LastUpdatedByUserId { get; set; }
    }
}