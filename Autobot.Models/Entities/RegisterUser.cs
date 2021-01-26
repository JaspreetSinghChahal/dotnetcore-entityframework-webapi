namespace Autobot.Models.Entities
{
    public class RegisterUser
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string OtherDetails { get; set; }
        public string Role { get; set; }
    }
}
