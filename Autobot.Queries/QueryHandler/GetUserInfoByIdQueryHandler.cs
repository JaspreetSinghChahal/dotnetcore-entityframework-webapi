using Autobot.Data.Model;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetUserInfoByIdQueryHandler : IRequestHandler<GetUserInfoByIdQuery, UserDetails>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;

        public GetUserInfoByIdQueryHandler(IUserManagerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get user details by userId
        /// Get user role;
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<UserDetails> Handle(GetUserInfoByIdQuery request, CancellationToken cancellationToken)
        {
            var getUserInfo = await _repository.GetUserInfoById(request.Id);
            var userRoles = await _repository.GetRoles(getUserInfo);
            var res = _mapper.Map<UserDetails>(getUserInfo);
            res.RoleName = userRoles.FirstOrDefault();
            return res;
        }
    }
}
