using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Infrastructure.Identity;
using Autobot.Queries.Query;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class GetTotalPointsQueryHandler : IRequestHandler<GetTotalPointsQuery, UserTotalPoints>
    {
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;
        public GetTotalPointsQueryHandler(IAutobotDbContext context, IUserManagerRepository repository)
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
        public async Task<UserTotalPoints> Handle(GetTotalPointsQuery request, CancellationToken cancellationToken)
        {
            DateTime lastResetDate = new DateTime(1900, 01, 01);
            var lastResetDates = from resetDate in _context.UserPointReset
                                 where resetDate.UserId == request.UserId
                                 select resetDate.ResetDateTime;

            if (lastResetDates != null && lastResetDates.Count() > 0)
            {
                lastResetDate = lastResetDates.Max();
            }
            var loyaltyPoints = (from user in _context.UserScans
                                 join promocode in _context.PromoCodes
                                 on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                 join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                 where user.UserId == request.UserId
                                 && user.IsSuccess == true
                                 && user.ScannedDateTime > lastResetDate
                                 select batch.LoyaltyPoints).Sum();

            var userInfo = await _repository.GetUserInfoById(request.UserId);

            var result = new UserTotalPoints()
            {
                UserName = userInfo.FirstName + " " + userInfo.LastName,
                Locations = userInfo.Location,
                Points = loyaltyPoints
            };

            return result;
        }
    }
}