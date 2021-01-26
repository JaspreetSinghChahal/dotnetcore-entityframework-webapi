using MediatR;

namespace Autobot.Commands.Command
{
    public class UpdateUserCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string OtherDetails { get; set; }
        public string DisplayPassword { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
    }
}