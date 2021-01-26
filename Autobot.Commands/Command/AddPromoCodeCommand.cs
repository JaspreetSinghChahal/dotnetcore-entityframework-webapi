using MediatR;
using System;

namespace Autobot.Commands.Command
{
    public class AddPromoCodeCommand : IRequest<string>
    {
        public string UserId { get; set; }
        public string BatchName { get; set; }
        public int BrandId { get; set; }
        public float LoyaltyPoints { get; set; }
        public int NoOfPromoCodes { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }
}

