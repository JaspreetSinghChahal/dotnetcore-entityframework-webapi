using MediatR;
using System.Collections.Generic;

namespace Autobot.Commands.Command
{
    public class DeleteUsersCommand : IRequest<string>
    {
        public List<string> UserIds { get; set; }
    }
}
