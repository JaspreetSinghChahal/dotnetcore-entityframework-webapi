using System;

namespace Autobot.Models.Entities
{
    public class UserScan
    {
        public long UserScanId { get; set; }
        public string UserId { get; set; }
        public long PromoCodeNumber { get; set; }
        public DateTime ScannedDateTime { get; set; }
        public bool IsSuccess { get; set; }
        public PromoCode PromoCode { get; set; }
    }
}
