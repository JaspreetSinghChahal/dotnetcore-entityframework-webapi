using System;

namespace Autobot.Models.Entities
{
    public class UserPointReset
    {
        public long UserPointResetId { get; set; }
        public string UserId { get; set; }
        public double PointsReset { get; set; }
        public DateTime ResetDateTime { get; set; }
        public string LastUpdatedByUserId { get; set; }
    }
}
