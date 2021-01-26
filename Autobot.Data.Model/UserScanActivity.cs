using System;

namespace Autobot.Data.Models
{
    public class UserScanActivity
    {
        public long UserScanId { get; set; }
        public long? PromoCodeNumber { get; set; }
        public string BrandName { get; set; }
        public double LoyaltyPoints { get; set; }
        public DateTime ScannedDateTime { get; set; }
        public bool IsSuccess { get; set; }
        public int FilteredCount { get; set; }
    }
}
