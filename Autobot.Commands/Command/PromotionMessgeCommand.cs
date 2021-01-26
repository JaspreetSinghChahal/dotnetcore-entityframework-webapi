using MediatR;
using System;

namespace Autobot.Commands.Command
{
    public class PromotionMessgeCommand : IRequest<string>
    {
        public string PromotionText { get; set; }
        public string PromotionFileName { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
