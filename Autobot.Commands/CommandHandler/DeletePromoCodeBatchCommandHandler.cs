using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class DeletePromoCodeBatchCommandHandler : IRequestHandler<DeletePromoCodeBatchCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public DeletePromoCodeBatchCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(DeletePromoCodeBatchCommand request, CancellationToken cancellationToken)
        {
            var batches = _context.PromoCodeBatch.Where(x => request.PromocodeBatchId.Contains(x.BatchId.ToString()));
            var promocodes = _context.PromoCodes.Where(x => request.PromocodeBatchId.Contains(x.BatchId.ToString()));
            if (batches == null || batches.Count() == 0)
            {
                return null;
            }

            foreach (var batch in batches)
            {
                batch.ExpirationDateTime = DateTime.Now;
            }

            _context.PromoCodeBatch.UpdateRange(batches);

            await _context.SaveChangesAsync(cancellationToken);
            return "success";
        }
    }
}

