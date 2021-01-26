using System;

namespace Autobot.Data.Model
{
    public class UserDetails
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayPassword { get; set; }
        public string Location { get; set; }
        public string OtherDetails { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int FilteredCount { get; set; }
        public DateTime TermsAndConditonsAcceptedOn { get; set; }
    }
}
