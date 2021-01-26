using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Models.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class ScanPromocodeCommandHandler : IRequestHandler<ScanPromocodeCommand, ScanResult>
    {
        private readonly IAutobotDbContext _context;

        public ScanPromocodeCommandHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Promocode are created in bulk. 
        /// Create multiple promocodes based on number given(NoOfPromoCodes)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ScanResult> Handle(ScanPromocodeCommand request, CancellationToken cancellationToken)
        {
            DateTime scannedDateTime = DateTime.Now;
            var scanResult = new ScanResult();

            // Get info from passed data
            var id = new Guid(request.PromoCodeId);
            // Get promocode info
            var promocode = _context.PromoCodes.Where(x => x.PromoCodeNumber == request.PromoCodeNumber && x.PromoCodeId == id && x.IsDeleted == false).FirstOrDefault();
            var batchInfo = _context.PromoCodeBatch.Where(x => x.BatchId == promocode.BatchId && x.IsDeleted == false).FirstOrDefault();

            // If promocode doesnot exist
            if (promocode == null || batchInfo == null)
            {
                return new ScanResult
                {
                    StatusCode = "400",
                    Response = "Invalid scan"
                };
            }

            // Log entry
            var scan = new UserScan()
            {
                UserId = request.UserId,
                PromoCodeNumber = request.PromoCodeNumber,
                ScannedDateTime = DateTime.Now
            };
            var userScans = _context.UserScans.Where(x => x.PromoCodeNumber == request.PromoCodeNumber && x.IsSuccess == true).FirstOrDefault();

            if (batchInfo.ExpirationDateTime < scannedDateTime)
            {
                scan.IsSuccess = false;
                scanResult.StatusCode = "400";
                scanResult.Response = "Promocode is expired";
            }
            else if (userScans != null)
            {
                scan.IsSuccess = false;
                scanResult.StatusCode = "400";
                scanResult.Response = "Promocode is already scanned";
            }
            else
            {
                scan.IsSuccess = true;
                scanResult.StatusCode = "200";
                scanResult.Response = "Successsfully scanned " + batchInfo.LoyaltyPoints + " points";
            }

            _context.UserScans.Add(scan);
            await _context.SaveChangesAsync(cancellationToken);

            return scanResult;
        }
    }
}