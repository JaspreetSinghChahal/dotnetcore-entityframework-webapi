using Microsoft.AspNetCore.Identity;
using System;

namespace Autobot.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// Text to hold Address of user
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Miscelaneous text for user
        /// </summary>
        public string OtherDetails { get; set; }
        public string DisplayPassword { get; set; }
        public bool IsTermsAndConditonsAccepted { get; set; }
        public DateTime TermsAndConditonsAcceptedOn { get; set; }
    }
}
