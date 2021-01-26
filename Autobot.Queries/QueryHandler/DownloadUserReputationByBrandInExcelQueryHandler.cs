using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Infrastructure.OpenXml;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class DownloadUserReputationByBrandInExcelQueryHandler : IRequestHandler<DownloadUserReputationByBrandInExcelQuery, byte[]>
    {
        private readonly IUserManagerRepository _repository;
        private readonly ISpreadsheetService _spreadsheetService;
        private readonly IAutobotDbContext _context;
        public DownloadUserReputationByBrandInExcelQueryHandler(ISpreadsheetService spreadsheetService, IAutobotDbContext context, IUserManagerRepository repository)
        {
            _spreadsheetService = spreadsheetService;
            _context = context;
            _repository = repository;
        }

        public async Task<byte[]> Handle(DownloadUserReputationByBrandInExcelQuery request, CancellationToken cancellationToken)
        {
            var users = _repository.GetUserList();
            var lastReset = (from resetDate in _context.UserPointReset
                             group resetDate by new { resetDate.UserId } into g
                             select new
                             {
                                 UserId = g.Key,
                                 ResetDateTime = g.Max(x => x.ResetDateTime)
                             }).ToList();

            var loyaltyPoints = await (from user in _context.UserScans
                                       join promocode in _context.PromoCodes
                                       on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                       join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                       where user.IsSuccess == true
                                       select new
                                       {
                                           user.UserId,
                                           batch.Brand.BrandName,
                                           user.ScannedDateTime,
                                           batch.LoyaltyPoints
                                       }).ToListAsync();

            List<UserBrandReputation> userPoints = new List<UserBrandReputation>();
            foreach (var user in users)
            {
                // Get last reset date
                var maxDate = new DateTime(1900, 01, 01);
                var date = lastReset.Where(x => x.UserId.UserId == user.Id).Select(x => x.ResetDateTime).ToList();
                if (date != null && date.Count() > 0)
                {
                    maxDate = date.Max();
                }
                var points = loyaltyPoints.Where(x => x.UserId == user.Id && x.ScannedDateTime > maxDate)
                              .GroupBy(x => new { x.UserId, x.BrandName })
                              .Select(x => new UserBrandReputation()
                              {
                                  UserId = x.Key.UserId,
                                  BrandName = x.Key.BrandName,
                                  UserName = user.FirstName + " " + user.LastName,
                                  ReputationPoint = x.Sum(y => y.LoyaltyPoints)
                              }).ToList();

                userPoints.AddRange(points);
            }

            if (_spreadsheetService.CreateSpreadsheet() == true)
            {
                // Create a few columns
                _spreadsheetService.CreateColumnWidth(1);
                _spreadsheetService.CreateColumnWidth(2);
                _spreadsheetService.CreateColumnWidth(3);
                _spreadsheetService.CreateColumnWidth(4);

                // Add column headers
                _spreadsheetService.AddHeader(new List<string>() { "Id", "UserName", "BrandName", "ReputationPoint" });

                foreach (var points in userPoints)
                {
                    _spreadsheetService.AddRow(new List<string>() { points.UserId, points.UserName, points.BrandName, points.ReputationPoint.ToString() });
                };
            }

            // Very important to close it!
            _spreadsheetService.CloseSpreadsheet();

            return _spreadsheetService.SpreadsheetStream.ToArray();
        }
    }
}