using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetPromoCodeBatchScanActivityByIdQueryHandler : IRequestHandler<GetPromoCodeBatchScanActivityByIdQuery, List<PromoCodeBatchScanActivity>>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _userManagerRepository;
        public GetPromoCodeBatchScanActivityByIdQueryHandler(IAutobotDbContext context, IUserManagerRepository userManagerRepository)
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
        public async Task<List<PromoCodeBatchScanActivity>> Handle(GetPromoCodeBatchScanActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var id = new Guid(request.BatchId);
            var userScans = (from batch in _context.PromoCodeBatch
                             join code in _context.PromoCodes
                             on batch.BatchId equals code.BatchId
                             join userScan in _context.UserScans
                             on code.PromoCodeNumber equals userScan.PromoCodeNumber
                             where batch.BatchId == id
                             select userScan).ToList();

            IEnumerable<PromoCodeBatchScanActivity> scans = new List<PromoCodeBatchScanActivity>();
            if (userScans != null && userScans.Count() > 0)
            {
                List<string> ids = userScans.Select(x => x.UserId).ToList();
                var usersinfo = await _userManagerRepository.GetUsersByIds(ids);

                scans = userScans.Join(usersinfo, c => c.UserId, cm => cm.Id, (c, cm) => new PromoCodeBatchScanActivity()
                {

                    UserId = c.UserId,
                    Username = cm.FirstName + " " + cm.LastName,
                    PromoCodeNumber = c.PromoCodeNumber,
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
                case "promocodenumber_desc":
                    scans = scans.OrderByDescending(s => s.PromoCodeNumber);
                    break;
                case "promocodenumber_asc":
                    scans = scans.OrderBy(s => s.PromoCodeNumber);
                    break;
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

            List<PromoCodeBatchScanActivity> scanActivities = new List<PromoCodeBatchScanActivity>();

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