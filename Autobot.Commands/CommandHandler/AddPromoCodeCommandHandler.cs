using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{

    public class AddPromoCodeCommandHandler : IRequestHandler<AddPromoCodeCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public AddPromoCodeCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(AddPromoCodeCommand request, CancellationToken cancellationToken)
        {
            Guid batchId = Guid.NewGuid();

            var batch = new PromoCodeBatch
            {
                BatchId = batchId,
                BatchName = request.BatchName,
                BrandId = request.BrandId,
                NoOfPromoCodes = request.NoOfPromoCodes,
                LoyaltyPoints = request.LoyaltyPoints,
                ExpirationDateTime = request.ExpirationDateTime,
                CreatedOn = DateTime.Now,
                CreatedByUserId = request.UserId,
                LastUpdatedOn = DateTime.Now,
                LastUpdatedBy = request.UserId,
                PromoCodes = new List<PromoCode>()
            };
            _context.PromoCodeBatch.Add(batch);

            for (int i = 0; i < request.NoOfPromoCodes; i++)
            {
                Guid promoCodeId = Guid.NewGuid();
                PromoCode entity = new PromoCode
                {
                    PromoCodeId = promoCodeId,
                    BatchId = batchId,
                    IsDeleted = false,
                    LastUpdatedOn = DateTime.Now,
                    LastUpdatedBy = request.UserId
                };
                batch.PromoCodes.Add(entity);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return batchId.ToString();
        }
    }
}

