using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Queries.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetUsersReputationPerBrandQueryHandler : IRequestHandler<GetUsersReputationPerBrandQuery, List<BrandReputation>>
    {
        private readonly IAutobotDbContext _context;

        public GetUsersReputationPerBrandQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<BrandReputation>> Handle(GetUsersReputationPerBrandQuery request, CancellationToken cancellationToken)
        {
            DateTime lastResetDate = new DateTime(1900, 01, 01);
            var lastResetDates = from resetDate in _context.UserPointReset
                                 where resetDate.UserId == request.Id
                                 select resetDate.ResetDateTime;

            if (lastResetDates != null && lastResetDates.Count() > 0)
            {
                lastResetDate = lastResetDates.Max();
            }

            var loyaltyPoints = from user in _context.UserScans
                                join promocode in _context.PromoCodes
                                on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                where user.UserId == request.Id
                                && user.IsSuccess == true
                                && user.ScannedDateTime > lastResetDate
                                group batch by batch.Brand.BrandName into g
                                select new BrandReputation()
                                {
                                    BrandName = g.Key,
                                    ReputationPoint = g.Sum(x => x.LoyaltyPoints)
                                };


            // Filter
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                loyaltyPoints = loyaltyPoints.Where(
                                     s => s.BrandName.ToLower().Contains(searchText)
                                          || s.ReputationPoint.ToString().Contains(searchText)
                                   );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "brandname_desc":
                    loyaltyPoints = loyaltyPoints.OrderByDescending(s => s.BrandName);
                    break;
                case "brandname_asc":
                    loyaltyPoints = loyaltyPoints.OrderBy(s => s.BrandName);
                    break;
                case "reputationpoint_desc":
                    loyaltyPoints = loyaltyPoints.OrderByDescending(s => s.ReputationPoint);
                    break;
                case "reputationpoint_asc":
                    loyaltyPoints = loyaltyPoints.OrderBy(s => s.ReputationPoint);
                    break;
                default:
                    loyaltyPoints = loyaltyPoints.OrderBy(s => s.ReputationPoint);
                    break;
            }

            int count = loyaltyPoints.Count();

            // Paginate
            loyaltyPoints = loyaltyPoints.Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize);

            List<BrandReputation> brandReputations = new List<BrandReputation>();

            if (loyaltyPoints != null)
            {
                // Update count
                brandReputations = loyaltyPoints.ToList();
                brandReputations.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(brandReputations);
        }
    }
}

