using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetPromoCodeScanActivityByIdQueryHandler : IRequestHandler<GetPromoCodeScanActivityByIdQuery, List<PromoCodeScanActivity>>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _userManagerRepository;
        public GetPromoCodeScanActivityByIdQueryHandler(IAutobotDbContext context, IUserManagerRepository userManagerRepository)
        {
            _context = context;
            _userManagerRepository = userManagerRepository;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<PromoCodeScanActivity>> Handle(GetPromoCodeScanActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var userScans = (from userScan in _context.UserScans
                             where userScan.PromoCodeNumber == request.PromocodeNumber
                             select userScan).ToList();

            IEnumerable<PromoCodeScanActivity> scans = new List<PromoCodeScanActivity>();
            if (userScans != null && userScans.Count() > 0)
            {
                List<string> ids = userScans.Select(x => x.UserId).ToList();
                var usersinfo = await _userManagerRepository.GetUsersByIds(ids);

                scans = userScans.Join(usersinfo, c => c.UserId, cm => cm.Id, (c, cm) => new PromoCodeScanActivity()
                {
                    UserId = c.UserId,
                    Username = cm.FirstName + " " + cm.LastName,
                    ScannedDateTime = c.ScannedDateTime,
                    IsSuccess = c.IsSuccess
                });
            }

            // Filter
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                scans = scans.Where(
                                     s => (s.Username.ToLower()).Contains(searchText)
                                          || s.ScannedDateTime.ToString().ToLower().Contains(searchText)
                                          || s.IsSuccess.ToSuccessText().ToLower().Contains(searchText)
                                   );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "username_desc":
                    scans = scans.OrderByDescending(s => s.Username);
                    break;
                case "username_asc":
                    scans = scans.OrderBy(s => s.Username);
                    break;
                case "scanneddatetime_desc":
                    scans = scans.OrderByDescending(s => s.ScannedDateTime);
                    break;
                case "scanneddatetime_asc":
                    scans = scans.OrderBy(s => s.ScannedDateTime);
                    break;
                case "issuccess_desc":
                    scans = scans.OrderByDescending(s => s.IsSuccess);
                    break;
                case "issuccess_asc":
                    scans = scans.OrderBy(s => s.IsSuccess);
                    break;
                default:
                    scans = scans.OrderByDescending(s => s.ScannedDateTime);
                    break;
            }

            int count = scans.Count();

            // Paginate
            scans = scans.Skip(request.PageNumber * request.PageSize)
                            .Take(request.PageSize);

            List<PromoCodeScanActivity> scanActivities = new List<PromoCodeScanActivity>();

            if (scanActivities != null)
            {
                // Update count
                scanActivities = scans.ToList();
                scanActivities.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(scanActivities);
        }

    }
}
