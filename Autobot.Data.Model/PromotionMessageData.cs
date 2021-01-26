using System;
using System.Collections.Generic;
using System.Text;

namespace Autobot.Data.Models
{
    public class PromotionMessageData
    {
        public int Id { get; set; }
        public string PromotionText { get; set; }
        public string PromotionFileName { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
