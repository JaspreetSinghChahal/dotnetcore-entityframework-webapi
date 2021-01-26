using Autobot.Data.Model;
using MediatR;

namespace Autobot.Queries.Query
{
    public class GetUserInfoByIdQuery : IRequest<UserDetails>
    {
        public string Id { get; set; }
    }
}
