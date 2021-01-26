using Autobot.Data.Models;
using MediatR;

namespace Autobot.Queries.Query
{
    public class GetTotalPointsQuery : IRequest<UserTotalPoints>
    {
        public string UserId { get; set; }
    }
}
