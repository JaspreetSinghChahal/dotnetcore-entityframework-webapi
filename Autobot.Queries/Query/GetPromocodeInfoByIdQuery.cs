using Autobot.Data.Models;
using MediatR;

namespace Autobot.Queries.Query
{
    public class GetPromocodeInfoByIdQuery : IRequest<PromoCodeDetails>
    {
        public long Id { get; set; }
    }
}
