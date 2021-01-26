using System;
using System.Collections.Generic;

namespace Autobot.Models.Entities
{
    public class PromoCodeBatch
    {
        public Guid BatchId { get; set; }
        public string BatchName { get; set; }
        public int BrandId { get; set; }
        public int NoOfPromoCodes { get; set; }
        public float LoyaltyPoints { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByUserId { get; set; }
        public List<PromoCode> PromoCodes { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public Brand Brand { get; set; }
    }
}
