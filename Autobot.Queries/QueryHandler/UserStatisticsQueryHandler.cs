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
    public class UserStatisticsQueryHandler : IRequestHandler<UserStatisticsQuery, UserStatistics>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;

        public UserStatisticsQueryHandler(IAutobotDbContext context, IUserManagerRepository repository)
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
        public async Task<UserStatistics> Handle(UserStatisticsQuery request, CancellationToken cancellationToken)
        {
            var statistics = new UserStatistics();
            var users = await _repository.GetUserWithGivenRole(Constants.User);
            statistics.TotalUsers = users.ToList().Count();

            var scansInLastTowWeek = (from scans in _context.UserScans
                                      where scans.ScannedDateTime >= DateTime.Now.AddDays(-14)
                                      && scans.IsSuccess == true
                                      select scans.UserId).ToList();
            var activeUsers = users.Where(a => scansInLastTowWeek.Any(c => c == a.Id)).ToList();
            statistics.ActiveUsers = activeUsers.ToList().Count();

            statistics.InActiveUsers = statistics.TotalUsers - statistics.ActiveUsers;

            var scannedPoints = from user in _context.UserScans
                                join promocode in _context.PromoCodes
                                on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                where promocode.IsDeleted == false
                                && user.IsSuccess == true
                                select batch.LoyaltyPoints;
            statistics.PointsScanned = scannedPoints.Sum();

            return statistics;
        }
    }
}
