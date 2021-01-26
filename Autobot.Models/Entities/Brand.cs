using System.Collections.Generic;

namespace Autobot.Models.Entities
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public List<PromoCodeBatch> PromoCodeBatch { get; set; }
    }
}
