using System;

namespace Autobot.Models.Entities
{
    public class PromotionMessage
    {
        public int Id { get; set; }
        public string PromotionText { get; set; }
        public string PromotionFileName { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
