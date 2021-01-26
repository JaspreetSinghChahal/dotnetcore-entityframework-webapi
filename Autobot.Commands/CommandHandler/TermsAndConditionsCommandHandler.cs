using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class TermsAndConditionsCommandHandler : IRequestHandler<TermsAndConditionsCommand, bool>
    {
        private readonly IAutobotDbContext _context;

        public TermsAndConditionsCommandHandler(IAutobotDbContext context)
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
        public async Task<bool> Handle(TermsAndConditionsCommand request, CancellationToken cancellationToken)
        {
            var termsAndConditions = _context.TermsAndConditions.FirstOrDefault();
            if (termsAndConditions == null)
            {
                _context.TermsAndConditions.Add(new TermsAndConditions()
                {
                    TermsAndConditionsText = request.TermsAndConditionsText,
                    LastUpdatedOn = DateTime.Now
                });
            }
            else
            {
                termsAndConditions.TermsAndConditionsText = request.TermsAndConditionsText;
                termsAndConditions.LastUpdatedOn = DateTime.Now;
                _context.TermsAndConditions.Update(termsAndConditions);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}