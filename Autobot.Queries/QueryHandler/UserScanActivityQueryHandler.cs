using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Queries.Query;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Autobot.Queries.QueryHandler
{
    public class UserScanActivityQueryHandler : IRequestHandler<UserScanActivityQuery, List<UserScanActivity>>
    {
        private readonly IAutobotDbContext _context;

        public UserScanActivityQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<UserScanActivity>> Handle(UserScanActivityQuery request, CancellationToken cancellationToken)
        {
            var userScanActivities = from user in _context.UserScans
                                     join promocode in _context.PromoCodes
                                     on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                     join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                     where user.UserId == request.Id
                                     select new UserScanActivity()
                                     {
                                         UserScanId = user.UserScanId,
                                         PromoCodeNumber = promocode.PromoCodeNumber,
                                         BrandName = batch.Brand.BrandName,
                                         LoyaltyPoints = batch.LoyaltyPoints,
                                         IsSuccess = user.IsSuccess,
                                         ScannedDateTime = user.ScannedDateTime
                                     };

            // Filter
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                userScanActivities = userScanActivities.Where(
                                     s => s.BrandName.ToLower().Contains(searchText)
                                          || s.PromoCodeNumber.ToString().ToLower().Contains(searchText)
                                          || s.PromoCodeNumber.ToString().ToLower().Contains(searchText)
                                          || s.LoyaltyPoints.ToString().ToLower().Contains(searchText)
                                          || s.ScannedDateTime.ToString().ToLower().Contains(searchText)
                                   );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "promocodenumber_desc":
                    userScanActivities = userScanActivities.OrderByDescending(s => s.PromoCodeNumber);
                    break;
                case "promocodenumber_asc":
                    userScanActivities = userScanActivities.OrderBy(s => s.PromoCodeNumber);
                    break;
                case "brandname_desc":
                    userScanActivities = userScanActivities.OrderByDescending(s => s.BrandName);
                    break;
                case "brandname_asc":
                    userScanActivities = userScanActivities.OrderBy(s => s.BrandName);
                    break;
                case "loyaltypoints_desc":
                    userScanActivities = userScanActivities.OrderByDescending(s => s.LoyaltyPoints);
                    break;
                case "loyaltypointse_asc":
                    userScanActivities = userScanActivities.OrderBy(s => s.LoyaltyPoints);
                    break;
                case "scanneddatetime_desc":
                    userScanActivities = userScanActivities.OrderByDescending(s => s.ScannedDateTime);
                    break;
                case "scanneddatetime_asc":
                    userScanActivities = userScanActivities.OrderBy(s => s.ScannedDateTime);
                    break;
                case "issuccess_desc":
                    userScanActivities = userScanActivities.OrderByDescending(s => s.IsSuccess);
                    break;
                case "issuccess_asc":
                    userScanActivities = userScanActivities.OrderBy(s => s.IsSuccess);
                    break;
                default:
                    userScanActivities = userScanActivities.OrderByDescending(s => s.ScannedDateTime);
                    break;
            }

            int count = userScanActivities.Count();

            // Paginate
            userScanActivities = userScanActivities.Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize);

            List<UserScanActivity> userScanActivity = new List<UserScanActivity>();

            if (userScanActivities != null)
            {
                // Update count
                userScanActivity = userScanActivities.ToList();
                userScanActivity.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(userScanActivity);
        }
    }
}
