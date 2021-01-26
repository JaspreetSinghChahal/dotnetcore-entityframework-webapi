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
    public class GetPromocodeBatchInfoByIdQueryHandler : IRequestHandler<GetPromocodeBatchInfoByIdQuery, PromoCodeBatchDetails>
    {
        private readonly IAutobotDbContext _context;

        public GetPromocodeBatchInfoByIdQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by userId
        /// Get user role;
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<PromoCodeBatchDetails> Handle(GetPromocodeBatchInfoByIdQuery request, CancellationToken cancellationToken)
        {
            Guid id = new Guid(request.Id);
            var promobatch = from batch in _context.PromoCodeBatch
                             where batch.BatchId == id
                             select batch;


            var promocodes = from code in _context.PromoCodes
                             join scan in _context.UserScans
                             on code.PromoCodeNumber equals scan.PromoCodeNumber
                             where scan.IsSuccess == true
                             && code.BatchId == id
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
                              LoyaltyPoints = b.LoyaltyPoints,
                              CreatedOn = b.CreatedOn,
                              NoOfPromoCodes = b.NoOfPromoCodes,
                              LastUpdatedOn = b.LastUpdatedOn,
                              NoOfPromoCodesScannedSuccessfully = result.NoOfPromoCodesScannedSuccessfully
                          };

            return await Task.FromResult(batches.FirstOrDefault());
        }
    }
}