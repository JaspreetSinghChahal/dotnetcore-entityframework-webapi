using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<Roles>>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRolesQueryHandler(IUserManagerRepository repository, IMapper mapper)
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
        public async Task<List<Roles>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roleDetails = await _repository.GetAllRoles();
            return _mapper.Map<List<Roles>>(roleDetails);
        }
    }
}
