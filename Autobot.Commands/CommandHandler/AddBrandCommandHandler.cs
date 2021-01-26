using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class AddBrandCommandHandler : IRequestHandler<AddBrandCommand, string>
    {
        private readonly IAutobotDbContext _context;

        public AddBrandCommandHandler(IAutobotDbContext context)
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
        public async Task<string> Handle(AddBrandCommand request, CancellationToken cancellationToken)
        {
            var brandInfo = from brand in _context.Brand
                            where brand.BrandName.ToLower() == request.BrandName
                            select brand;

            if (brandInfo != null && brandInfo.Count() > 0)
            {
                return null;
            }

            var batch = new Brand
            {
                BrandName = request.BrandName
            };
            _context.Brand.Add(batch);
            await _context.SaveChangesAsync(cancellationToken);
            return batch.BrandId.ToString();
        }
    }
}

