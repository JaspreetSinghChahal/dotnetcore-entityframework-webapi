using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using MediatR;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class PromotionMessgeCommandHandler : IRequestHandler<PromotionMessgeCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public PromotionMessgeCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(PromotionMessgeCommand request, CancellationToken cancellationToken)
        {
            var data = _context.PromotionMessage.FirstOrDefault();
            if (data == null)
            {
                _context.PromotionMessage.Add(new PromotionMessage()
                {
                    PromotionText = request.PromotionText,
                    PromotionFileName = request.PromotionFileName,
                    LastUpdatedOn = DateTime.Now,
                    LastUpdatedBy = request.LastUpdatedBy
                });
            }
            else
            {
                data.PromotionText = request.PromotionText;
                data.PromotionFileName = request.PromotionFileName;
                data.LastUpdatedOn = DateTime.Now;
                data.LastUpdatedBy = request.LastUpdatedBy;
                _context.PromotionMessage.Update(data);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return "success";
        }
    }
}
