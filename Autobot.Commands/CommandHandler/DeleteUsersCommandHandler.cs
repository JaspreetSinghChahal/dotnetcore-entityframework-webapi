using Autobot.Commands.Command;
using Autobot.Infrastructure.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class DeleteUsersCommandHandler : IRequestHandler<DeleteUsersCommand, string>
    {
        private readonly IUserManagerRepository _repository;

        public DeleteUsersCommandHandler(IUserManagerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Promocode are created in bulk. 
        /// Create multiple promocodes based on number given(NoOfPromoCodes)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> Handle(DeleteUsersCommand request, CancellationToken cancellationToken)
        {
            var data = await _repository.DeleteUsersAsync(request.UserIds);
            if (data == false)
            {
                return "falure";
            }
            else
            {
                return "success";
            }
        }
    }
}