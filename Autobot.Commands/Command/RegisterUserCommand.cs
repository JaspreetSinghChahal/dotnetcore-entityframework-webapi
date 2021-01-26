using MediatR;

namespace Autobot.Commands.Command
{
    public class RegisterUserCommand : IRequest<string>
    {
        /// <summary>
        /// Acts as Username to uniquely identify users
        /// </summary>
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        /// <summary>
        /// Text of hold Address of user
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Miscelaneous text for user
        /// </summary>
        public string OtherDetails { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
    }
}
