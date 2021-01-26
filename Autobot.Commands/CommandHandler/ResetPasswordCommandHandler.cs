using Autobot.Commands.Command;
using Autobot.Infrastructure.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserManagerRepository _repository;

        public ResetPasswordCommandHandler(IUserManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var userInfo = await _repository.GetUserInfoById(request.UserId);
            if (userInfo == null)
            {
                return false;
            }

            var res = await _repository.ResetPasswordWithTokenAsync(userInfo, request.Password, request.Token);
            if (res == true)
            {
                userInfo.DisplayPassword = request.Password;
                await _repository.UpdateUserAsync(userInfo);
            }
            return true;
        }
    }
}
