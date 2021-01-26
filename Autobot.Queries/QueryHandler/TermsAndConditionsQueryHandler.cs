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
    public class TermsAndConditionsQueryHandler : IRequestHandler<TermsAndConditionsQuery, TermsAndConditions>
    {
        private readonly IAutobotDbContext _context;
        public TermsAndConditionsQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<TermsAndConditions> Handle(TermsAndConditionsQuery request, CancellationToken cancellationToken)
        {
            var termsAndConditions = (await (from message in _context.TermsAndConditions
                                             select new TermsAndConditions()
                                             {
                                                 TermsAndConditionsText = message.TermsAndConditionsText,
                                                 LastUpdatedOn = message.LastUpdatedOn
                                             }).ToListAsync()).FirstOrDefault();
            return termsAndConditions;
        }
    }
}