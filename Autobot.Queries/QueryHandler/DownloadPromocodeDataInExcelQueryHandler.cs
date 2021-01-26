using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Infrastructure.OpenXml;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class DownloadPromocodeDataInExcelQueryHandler : IRequestHandler<DownloadPromocodeDataInExcelQuery, byte[]>
    {
        private readonly IUserManagerRepository _repository;
        private readonly ISpreadsheetService _spreadsheetService;
        private readonly IAutobotDbContext _context;
        public DownloadPromocodeDataInExcelQueryHandler(ISpreadsheetService spreadsheetService, IAutobotDbContext context, IUserManagerRepository repository)
        {
            _spreadsheetService = spreadsheetService;
            _context = context;
            _repository = repository;
        }

        public async Task<byte[]> Handle(DownloadPromocodeDataInExcelQuery request, CancellationToken cancellationToken)
        {
            var promocodes = await (from s in _context.PromoCodes
                                    join b in _context.PromoCodeBatch on s.BatchId equals b.BatchId
                                    join c in _context.UserScans on s.PromoCodeNumber equals c.PromoCodeNumber into details
                                    from m in details.DefaultIfEmpty()
                                    where (m.UserId == null || m.IsSuccess == true)
                                    && s.IsDeleted == false && b.IsDeleted == false
                                    select new PromoCodeDetails()
                                    {
                                        PromoCodeNumber = s.PromoCodeNumber,
                                        PromoCodeId = s.PromoCodeId,
                                        BatchId = s.BatchId,
                                        BatchName = b.BatchName,
                                        BrandName = b.Brand.BrandName,
                                        LoyaltyPoints = b.LoyaltyPoints,
                                        ExpirationDateTime = b.ExpirationDateTime,
                                        IsScanned = m.IsSuccess,
                                        ScannedByUserId = m.UserId,
                                        IsDeleted = s.IsDeleted,
                                        CreatedOn = b.CreatedOn,
                                        CreatedByUserId = b.CreatedByUserId,
                                        LastUpdatedOn = s.LastUpdatedOn,
                                        LastUpdatedBy = s.LastUpdatedBy
                                    }).ToListAsync();

            var userIds = promocodes.Where(x => string.IsNullOrEmpty(x.ScannedByUserId) == false).Select(x => x.ScannedByUserId).ToList();
            var users = await _repository.GetUsersByIds(userIds);
            foreach (var promocode in promocodes)
            {
                if (!string.IsNullOrEmpty(promocode.ScannedByUserId))
                {
                    promocode.ScannedByUser = users.Where(x => x.Id == promocode.ScannedByUserId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                }
            }

            if (_spreadsheetService.CreateSpreadsheet() == true)
            {
                // Create a few columns
                _spreadsheetService.CreateColumnWidth(1);
                _spreadsheetService.CreateColumnWidth(2);
                _spreadsheetService.CreateColumnWidth(3);
                _spreadsheetService.CreateColumnWidth(4);
                _spreadsheetService.CreateColumnWidth(5);
                _spreadsheetService.CreateColumnWidth(6);
                _spreadsheetService.CreateColumnWidth(7);
                _spreadsheetService.CreateColumnWidth(8);
                _spreadsheetService.CreateColumnWidth(9);
                _spreadsheetService.CreateColumnWidth(10);
                _spreadsheetService.CreateColumnWidth(11);

                // Add column headers
                _spreadsheetService.AddHeader(new List<string>() { "PromoCodeId", "PromoCodeNumber", "BatchId", "BatchName", "BrandName", "ExpirationDateTime", "LoyaltyPoints", "CreatedByUserId", "CreatedOn", "ScannedByUserId", "ScannedByUser" });

                foreach (var promocode in promocodes)
                {
                    _spreadsheetService.AddRow(new List<string>() { promocode.PromoCodeId.ToString(), promocode.PromoCodeNumber.ToString(), promocode.BatchId.ToString(), promocode.BatchName, promocode.BrandName, promocode.ExpirationDateTime.ToString(), promocode.LoyaltyPoints.ToString(), promocode.CreatedByUserId, promocode.CreatedOn.ToString(), promocode.ScannedByUserId, promocode.ScannedByUser });
                };
            }

            // Very important to close it!
            _spreadsheetService.CloseSpreadsheet();

            return _spreadsheetService.SpreadsheetStream.ToArray();
        }
    }
}