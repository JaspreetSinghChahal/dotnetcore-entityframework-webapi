using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class UserPointResetCommandHandler : IRequestHandler<UserPointResetCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public UserPointResetCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(UserPointResetCommand request, CancellationToken cancellationToken)
        {
            var points = 0.00;
            var loyaltyPoints = from user in _context.UserScans
                                join promocode in _context.PromoCodes
                                on user.PromoCodeNumber equals promocode.PromoCodeNumber
                                join batch in _context.PromoCodeBatch on promocode.BatchId equals batch.BatchId
                                where user.UserId == request.UserId
                                && user.IsSuccess == true
                                select batch.LoyaltyPoints;

            if (loyaltyPoints != null && loyaltyPoints.Count() > 0)
            {
                points = loyaltyPoints.Sum();
            }

            DateTime resetDateTime = DateTime.Now;

            var userResetPoint = new UserPointReset()
            {
                UserId = request.UserId,
                LastUpdatedByUserId = request.LastUpdatedByUserId,
                PointsReset = points,
                ResetDateTime = resetDateTime
            };

            _context.UserPointReset.Add(userResetPoint);
            await _context.SaveChangesAsync(cancellationToken);

            return userResetPoint.UserPointResetId.ToString();
        }
    }
}