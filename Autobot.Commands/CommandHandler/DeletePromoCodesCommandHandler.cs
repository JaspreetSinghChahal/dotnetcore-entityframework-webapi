using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class DeletePromoCodesCommandHandler : IRequestHandler<DeletePromoCodesCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public DeletePromoCodesCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(DeletePromoCodesCommand request, CancellationToken cancellationToken)
        {
            var data = _context.PromoCodes.Where(x => request.PromocodeNumbers.Contains(x.PromoCodeNumber));
            if (data == null)
            {
                return "falure";
            }
            else
            {
                _context.PromoCodes.RemoveRange(data);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return "success";
        }
    }
}

