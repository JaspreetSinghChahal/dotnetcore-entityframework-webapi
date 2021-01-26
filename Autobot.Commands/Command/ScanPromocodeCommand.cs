using Autobot.Data.Models;
using MediatR;

namespace Autobot.Commands.Command
{
    public class ScanPromocodeCommand : IRequest<ScanResult>
    {
        public string UserId { get; set; }
        public long PromoCodeNumber { get; set; }
        public string PromoCodeId { get; set; }
    }
}
