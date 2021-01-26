using System;

namespace Autobot.Data.Models
{
    public class UserResetPointDetails
    {
        public DateTime ResetDateTime { get; set; }
        public double ResetPoints { get; set; }
        public int FilteredCount { get; set; }
    }
}
