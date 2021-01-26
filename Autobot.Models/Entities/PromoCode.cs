using System;
using System.Collections.Generic;

namespace Autobot.Models.Entities
{
    public class PromoCode
    {
        public Guid BatchId { get; set; }
        public long PromoCodeNumber { get; set; }
        public Guid PromoCodeId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public List<UserScan> UserScans { get; set; }
        public PromoCodeBatch PromoCodeBatch { get; set; }
    }
}
