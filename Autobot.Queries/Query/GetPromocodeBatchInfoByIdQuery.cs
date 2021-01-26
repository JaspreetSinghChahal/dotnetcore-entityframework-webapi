using Autobot.Data.Models;
using MediatR;

namespace Autobot.Queries.Query
{
    public class GetPromocodeBatchInfoByIdQuery : IRequest<PromoCodeBatchDetails>
    {
        public string Id { get; set; }
    }
}