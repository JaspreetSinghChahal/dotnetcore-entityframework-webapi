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
    public class GetPromoCodesListQueryHandler : IRequestHandler<GetPromoCodesListQuery, List<PromoCodeDetails>>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;
        public GetPromoCodesListQueryHandler(IAutobotDbContext context, IUserManagerRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<PromoCodeDetails>> Handle(GetPromoCodesListQuery request, CancellationToken cancellationToken)
        {
            // Get promocodes
            var promocodes = from s in _context.PromoCodes
                             join b in _context.PromoCodeBatch on s.BatchId equals b.BatchId
                             join c in _context.UserScans on s.PromoCodeNumber equals c.PromoCodeNumber into details
                             from m in details.Where(f => f.IsSuccess == true).DefaultIfEmpty()
                             select new PromoCodeDetails()
                             {
                                 PromoCodeNumber = s.PromoCodeNumber,
                                 PromoCodeId = s.PromoCodeId,
                                 BatchId = s.BatchId,
                                 BatchName = b.BatchName,
                                 BrandName = b.Brand.BrandName,
                                 LoyaltyPoints = b.LoyaltyPoints,
                                 ExpirationDateTime = b.ExpirationDateTime,
                                 IsExpired = b.ExpirationDateTime < DateTime.Now ? true : false,
                                 IsScanned = m.IsSuccess,
                                 ScannedByUserId = m.UserId,
                                 IsDeleted = s.IsDeleted,
                                 CreatedOn = b.CreatedOn,
                                 CreatedByUserId = b.CreatedByUserId,
                                 LastUpdatedOn = s.LastUpdatedOn,
                                 LastUpdatedBy = s.LastUpdatedBy
                             };

            // Filter
            if (!String.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter;
                promocodes = promocodes.Where(
                    s => s.BatchName.Contains(searchText)
                                       || s.BatchName.ToLower().Contains(searchText)
                                       || s.BrandName.ToLower().Contains(searchText)
                                       || s.PromoCodeNumber.ToString().ToLower().Contains(searchText)
                                       );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "promocodenumber_desc":
                    promocodes = promocodes.OrderByDescending(s => s.PromoCodeNumber);
                    break;
                case "promocodenumber_asc":
                    promocodes = promocodes.OrderBy(s => s.PromoCodeNumber);
                    break;
                case "batchname_desc":
                    promocodes = promocodes.OrderByDescending(s => s.BatchName);
                    break;
                case "batchname_asc":
                    promocodes = promocodes.OrderBy(s => s.BatchName);
                    break;
                case "brandname_desc":
                    promocodes = promocodes.OrderByDescending(s => s.BrandName);
                    break;
                case "brandname_asc":
                    promocodes = promocodes.OrderBy(s => s.BrandName);
                    break;
                case "loyaltypoints_desc":
                    promocodes = promocodes.OrderByDescending(s => s.LoyaltyPoints);
                    break;
                case "loyaltypoints_asc":
                    promocodes = promocodes.OrderBy(s => s.LoyaltyPoints);
                    break;
                case "expirationdatetime_desc":
                    promocodes = promocodes.OrderByDescending(s => s.ExpirationDateTime);
                    break;
                case "expirationdatetime_asc":
                    promocodes = promocodes.OrderBy(s => s.ExpirationDateTime);
                    break;
                case "isscanned_desc":
                    promocodes = promocodes.OrderByDescending(s => s.IsScanned);
                    break;
                case "isscanned_asc":
                    promocodes = promocodes.OrderBy(s => s.IsScanned);
                    break;
                case "createdon_desc":
                    promocodes = promocodes.OrderByDescending(s => s.CreatedOn);
                    break;
                case "createdon_asc":
                    promocodes = promocodes.OrderBy(s => s.CreatedOn);
                    break;
                case "lastupdatedon_desc":
                    promocodes = promocodes.OrderByDescending(s => s.LastUpdatedOn);
                    break;
                case "lastupdatedon_asc":
                    promocodes = promocodes.OrderBy(s => s.LastUpdatedOn);
                    break;
                default:
                    promocodes = promocodes.OrderBy(s => s.LastUpdatedOn);
                    break;
            }

            int count = promocodes.Count();

            // Paginate
            promocodes = promocodes.Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize);

            List<PromoCodeDetails> promocodeList = new List<PromoCodeDetails>();

            if (promocodes != null)
            {
                // Update count
                promocodeList = promocodes.ToList();
                promocodeList.ForEach(x => x.FilteredCount = count);
            }

            // Get user Info
            var userIds = promocodeList.Where(x => string.IsNullOrEmpty(x.ScannedByUserId) == false).Select(x => x.ScannedByUserId).ToList();
            var users = await _repository.GetUsersByIds(userIds);
            foreach (var promocode in promocodeList)
            {
                if (!string.IsNullOrEmpty(promocode.ScannedByUserId))
                {
                    promocode.ScannedByUser = users.Where(x => x.Id == promocode.ScannedByUserId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                }
            }

            return promocodeList;
        }
    }
}
