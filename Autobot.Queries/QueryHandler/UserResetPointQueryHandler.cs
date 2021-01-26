using Autobot.Data.Interfaces;
using Autobot.Data.Models;
using Autobot.Queries.Query;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class UserResetPointQueryHandler : IRequestHandler<UserResetPointQuery, List<UserResetPointDetails>>
    {
        private readonly IAutobotDbContext _context;

        public UserResetPointQueryHandler(IAutobotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<List<UserResetPointDetails>> Handle(UserResetPointQuery request, CancellationToken cancellationToken)
        {
            var userPointReset = from reset in _context.UserPointReset
                                 where reset.UserId == request.Id
                                 select new UserResetPointDetails()
                                 {
                                     ResetPoints = reset.PointsReset,
                                     ResetDateTime = reset.ResetDateTime,
                                 };

            // Filter
            if (!string.IsNullOrEmpty(request.Filter))
            {
                var searchText = request.Filter.ToLower();
                userPointReset = userPointReset.Where(
                                     s => s.ResetDateTime.ToString().Contains(searchText)
                                          || s.ResetPoints.ToString().ToLower().Contains(searchText)
                                   );
            }
            // Sort
            switch (request.SortColumn.ToLower() + "_" + "" + request.SortOrder.ToLower())
            {
                case "resetdatetime_desc":
                    userPointReset = userPointReset.OrderByDescending(s => s.ResetDateTime);
                    break;
                case "resetdatetime_asc":
                    userPointReset = userPointReset.OrderBy(s => s.ResetDateTime);
                    break;
                case "resetpoints_desc":
                    userPointReset = userPointReset.OrderByDescending(s => s.ResetPoints);
                    break;
                case "resetpoints_asc":
                    userPointReset = userPointReset.OrderBy(s => s.ResetPoints);
                    break;
                default:
                    userPointReset = userPointReset.OrderByDescending(s => s.ResetDateTime);
                    break;
            }

            int count = userPointReset.Count();

            // Paginate
            userPointReset = userPointReset.Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize);

            List<UserResetPointDetails> userResetPointDetails = new List<UserResetPointDetails>();

            if (userPointReset != null)
            {
                // Update count
                userResetPointDetails = userPointReset.ToList();
                userResetPointDetails.ForEach(x => x.FilteredCount = count);
            }

            return await Task.FromResult(userResetPointDetails);
        }
    }
}
