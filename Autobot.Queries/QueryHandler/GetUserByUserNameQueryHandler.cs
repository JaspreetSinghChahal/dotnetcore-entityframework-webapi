using Autobot.Data.Model;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, UserDetails>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;

        public GetUserByUserNameQueryHandler(IUserManagerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<UserDetails> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var userDetails = await _repository.GetUserDetailByUserName(request.UserName);
            var res = _mapper.Map<UserDetails>(userDetails);
            return res;
        }
    }
}
