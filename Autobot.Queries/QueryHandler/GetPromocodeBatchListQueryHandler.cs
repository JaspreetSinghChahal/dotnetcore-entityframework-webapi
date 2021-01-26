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
    public class GetPromocodeBatchListQueryHandler : IRequestHandler<GetPromocodeBatchListQuery, List<PromoCodeBatchDetails>>
    {
        private readonly IAutobotDbContext _context;

        public GetPromocodeBatchListQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<PromoCodeBatchDetails>> Handle(GetPromocodeBatchListQuery request, CancellationToken cancellationToken)
        {
            var promobatch = from batch in _context.PromoCodeBatch
                             select batch;

            var promocodes = from code in _context.PromoCodes
                             join scan in _context.UserScans
                             on code.PromoCodeNumber equals scan.PromoCodeNumber
                             where scan.IsSuccess == true
                             group scan by code.BatchId into g
                             select new
                             {
                                 BatchId = g.Key,
                                 NoOfPromoCodesScannedSuccessfully = g.Count()
                             };

            var batches = from b in promobatch
                          join c in promocodes
                          on b.BatchId equals c.BatchId into scan
                          from result in scan.DefaultIfEmpty()
                          select new PromoCodeBatchDetails()
                          {
                              BatchId = b.BatchId,
                              BatchName = b.BatchName,
                              BrandId = b.BrandId,
                              BrandName = b.Brand.BrandName,
                              ExpirationDateTime = b.ExpirationDateTime,
                              IsExpired = b.ExpirationDateTime < DateTime.Now ? true : false,
                              LoyaltyPoints = b.LoyaltyPoints,
                              NoOfPromoCodes = b.NoOfPromoCodes,
                              LastUpdatedOn = b.LastUpdatedOn,
                              NoOfPromoCodesScannedSuccessfully = result.NoOfPromoCodesScannedSuccessfully
                          };


            // Filter
            if (!String.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                batches = batches.Where(
                    s => s.BatchName.ToLower().Contains(searchText)
                                               || s.BrandName.ToLower().Contains(searchText)
                                               || s.LoyaltyPoints.ToString().ToLower().Contains(searchText)
                                               || s.NoOfPromoCodes.ToString().ToLower().Contains(searchText)
                                               || s.ExpirationDateTime.ToString().ToLower().Contains(searchText)
                                               || s.NoOfPromoCodesScannedSuccessfully.ToString().ToLower().Contains(searchText)
                                               );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "batchname_desc":
                    batches = batches.OrderByDescending(s => s.BatchName);
                    break;
                case "batchname_asc":
                    batches = batches.OrderBy(s => s.BatchName);
                    break;
                case "brandname_desc":
                    batches = batches.OrderByDescending(s => s.BrandName);
                    break;
                case "brandname_asc":
                    batches = batches.OrderBy(s => s.BrandName);
                    break;
                case "noofpromocodes_desc":
                    batches = batches.OrderByDescending(s => s.NoOfPromoCodes);
                    break;
                case "noofpromocodes_asc":
                    batches = batches.OrderBy(s => s.NoOfPromoCodes);
                    break;
                case "loyaltypoints_desc":
                    batches = batches.OrderByDescending(s => s.LoyaltyPoints);
                    break;
                case "loyaltypoints_asc":
                    batches = batches.OrderBy(s => s.LoyaltyPoints);
                    break;
                case "expirationdatetime_desc":
                    batches = batches.OrderByDescending(s => s.ExpirationDateTime);
                    break;
                case "expirationdatetime_asc":
                    batches = batches.OrderBy(s => s.ExpirationDateTime);
                    break;
                case "noofpromocodesscannedsuccessfully_desc":
                    batches = batches.OrderByDescending(s => s.NoOfPromoCodesScannedSuccessfully);
                    break;
                case "noofpromocodesscannedsuccessfully_asc":
                    batches = batches.OrderBy(s => s.NoOfPromoCodesScannedSuccessfully);
                    break;
                case "createdon_desc":
                    batches = batches.OrderByDescending(s => s.CreatedOn);
                    break;
                case "createdon_asc":
                    batches = batches.OrderBy(s => s.CreatedOn);
                    break;
                case "lastupdatedon_desc":
                    batches = batches.OrderByDescending(s => s.LastUpdatedOn);
                    break;
                case "lastupdatedon_asc":
                    batches = batches.OrderBy(s => s.LastUpdatedOn);
                    break;
                default:
                    batches = batches.OrderByDescending(s => s.LastUpdatedOn);
                    break;
            }

            int count = batches.Count();

            // Paginate
            batches = batches.Skip(request.PageNumber * request.PageSize)
                            .Take(request.PageSize);

            List<PromoCodeBatchDetails> promocodeList = new List<PromoCodeBatchDetails>();

            if (batches != null)
            {
                // Update count
                promocodeList = batches.ToList();
                promocodeList.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(promocodeList);
        }
    }
}