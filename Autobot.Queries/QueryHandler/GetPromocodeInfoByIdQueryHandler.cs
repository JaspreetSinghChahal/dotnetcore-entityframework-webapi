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
    public class GetPromocodeInfoByIdQueryHandler : IRequestHandler<GetPromocodeInfoByIdQuery, PromoCodeDetails>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;

        public GetPromocodeInfoByIdQueryHandler(IAutobotDbContext context, IUserManagerRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        /// <summary>
        /// Get user details by userId
        /// Get user role;
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<PromoCodeDetails> Handle(GetPromocodeInfoByIdQuery request, CancellationToken cancellationToken)
        {
            // Promocode info
            var promocode = (from s in _context.PromoCodes
                             join b in _context.PromoCodeBatch on s.BatchId equals b.BatchId
                             where s.PromoCodeNumber == request.Id
                             select new PromoCodeDetails()
                             {
                                 PromoCodeNumber = s.PromoCodeNumber,
                                 PromoCodeId = s.PromoCodeId,
                                 BatchId = s.BatchId,
                                 BatchName = b.BatchName,
                                 BrandId = b.BrandId,
                                 BrandName = b.Brand.BrandName,
                                 LoyaltyPoints = b.LoyaltyPoints,
                                 ExpirationDateTime = b.ExpirationDateTime,
                                 IsDeleted = s.IsDeleted,
                                 CreatedOn = b.CreatedOn,
                                 CreatedByUserId = b.CreatedByUserId,
                                 LastUpdatedOn = s.LastUpdatedOn,
                                 LastUpdatedBy = s.LastUpdatedBy
                             }).FirstOrDefault();

            if (promocode == null)
            {
                return null;
            }

            // Scan Info
            var scans = from c in _context.UserScans
                        where c.PromoCodeNumber == promocode.PromoCodeNumber
                        select new
                        {
                            c.IsSuccess,
                            ScannedByUserId = c.UserId
                        };

            if (scans != null)
            {
                var successsFullScan = scans.Where(x => x.IsSuccess == true).FirstOrDefault();
                if (successsFullScan != null)
                {
                    promocode.IsScanned = true;
                    promocode.ScannedByUserId = successsFullScan.ScannedByUserId;
                }
            }

            if (promocode.IsScanned == true && promocode.ScannedByUserId != null)
            {
                var usersinfo = await _repository.GetUserInfoById(promocode.ScannedByUserId);
                promocode.ScannedByUser = usersinfo.FirstName + " " + usersinfo.LastName;
            }

            return await Task.FromResult(promocode);
        }
    }
}