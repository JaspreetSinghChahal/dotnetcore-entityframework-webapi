using Autobot.Commands.Command;
using Autobot.Infrastructure.Identity;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IUserManagerRepository _repository;

        public UpdateUserCommandHandler(IUserManagerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Update user details
        /// Reset password
        /// Add and remove role
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _repository.GetUserInfoById(request.Id);
            if (userInfo == null)
            {
                return null;
            }

            userInfo.FirstName = request.FirstName;
            userInfo.LastName = request.LastName;
            userInfo.Location = request.Location;
            userInfo.OtherDetails = request.OtherDetails;
            userInfo.PhoneNumber = request.PhoneNumber;
            userInfo.UserName = request.PhoneNumber;
            userInfo.DisplayPassword = request.DisplayPassword;
            userInfo.Email = request.Email;

            var isUserUpdated = await _repository.UpdateUserAsync(userInfo);
            if (isUserUpdated)
            {
                await _repository.ResetPasswordAsync(userInfo, request.DisplayPassword);
                var roles = await _repository.GetRoles(userInfo);
                if (!roles.Contains(request.RoleName))
                {
                    _repository.RemoveUserFromRoles(roles.ToList(), userInfo);
                    _repository.AddUserToRole(request.RoleName, userInfo);
                }
            }
            return userInfo.Id;
        }
    }
}