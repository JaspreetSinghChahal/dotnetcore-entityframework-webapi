using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class PromotionMessageQueryHandler : IRequestHandler<PromotionMessageQuery, PromotionMessage>
    {
        private readonly IAutobotDbContext _context;
        public PromotionMessageQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<PromotionMessage> Handle(PromotionMessageQuery request, CancellationToken cancellationToken)
        {
            var promotionMessage = (await (from message in _context.PromotionMessage
                                           select new PromotionMessage()
                                           {
                                               Id = message.Id,
                                               PromotionText = message.PromotionText,
                                               PromotionFileName = message.PromotionFileName,
                                               LastUpdatedBy = message.LastUpdatedBy,
                                               LastUpdatedOn = message.LastUpdatedOn
                                           }).ToListAsync()).FirstOrDefault();
            return promotionMessage;
        }
    }
}