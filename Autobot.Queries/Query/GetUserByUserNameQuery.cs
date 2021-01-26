using Autobot.Data.Model;
using MediatR;

namespace Autobot.Queries.Query
{
    public class GetUserByUserNameQuery : IRequest<UserDetails>
    {
        public string UserName { get; set; }
    }
}
