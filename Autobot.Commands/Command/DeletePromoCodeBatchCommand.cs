using MediatR;
using System.Collections.Generic;

namespace Autobot.Commands.Command
{
    public class DeletePromoCodeBatchCommand : IRequest<string>
    {
        public List<string> PromocodeBatchId { get; set; }
    }
}
