using Autobot.Data.Models;
using MediatR;
using System.Collections.Generic;

namespace Autobot.Queries.Query
{
    public class GetPromoCodesListQuery : IRequest<List<PromoCodeDetails>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }
}
