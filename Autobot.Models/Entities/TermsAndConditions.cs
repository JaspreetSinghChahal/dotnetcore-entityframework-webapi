using System;

namespace Autobot.Models.Entities
{
    public class TermsAndConditions
    {
        public int Id { get; set; }
        public string TermsAndConditionsText { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
