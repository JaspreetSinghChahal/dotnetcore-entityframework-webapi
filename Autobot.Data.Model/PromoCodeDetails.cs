using System;

namespace Autobot.Data.Models
{
    public class PromoCodeDetails
    {
        public long PromoCodeNumber { get; set; }
        public Guid PromoCodeId { get; set; }
        public Guid BatchId { get; set; }
        public int BrandId { get; set; }
        public string BatchName { get; set; }
        public string BrandName { get; set; }
        public float LoyaltyPoints { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public bool IsScanned { get; set; }
        public bool IsExpired { get; set; }
        public string ScannedByUserId { get; set; }
        public string ScannedByUser { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public int FilteredCount { get; set; }
    }
}
