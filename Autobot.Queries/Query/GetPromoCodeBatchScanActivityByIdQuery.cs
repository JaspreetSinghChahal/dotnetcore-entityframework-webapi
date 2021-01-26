using Autobot.Data.Models;
using MediatR;
using System.Collections.Generic;

namespace Autobot.Queries.Query
{
    public class GetPromoCodeBatchScanActivityByIdQuery : IRequest<List<PromoCodeBatchScanActivity>>
    {
        public string BatchId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}