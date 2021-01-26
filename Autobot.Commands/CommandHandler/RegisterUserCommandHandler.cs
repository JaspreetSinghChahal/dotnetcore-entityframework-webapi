using Autobot.Commands.Command;
using Autobot.Infrastructure.Identity;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;

        public RegisterUserCommandHandler(IUserManagerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var register = _mapper.Map<RegisterUser>(request);
            return await _repository.CreateUserAsync(register);
        }
    }
}
