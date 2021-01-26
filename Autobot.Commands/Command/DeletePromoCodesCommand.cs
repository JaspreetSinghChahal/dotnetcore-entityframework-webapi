using MediatR;
using System.Collections.Generic;

namespace Autobot.Commands.Command
{
    public class DeletePromoCodesCommand : IRequest<string>
    {
        public List<long> PromocodeNumbers { get; set; }
    }
}
