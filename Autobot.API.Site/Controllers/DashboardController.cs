using Autobot.API.Site.Common;
using Autobot.Commands.Command;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autobot.API.Site.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Count of all users
        /// Count of Active and Inactive user -- Users who have/havenot scanned in last 14 days
        /// Total points scanned so far
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("GetUserStatistics")]
        public async Task<ActionResult> GetUserStatistics()
        {
            UserStatisticsQuery query = new UserStatisticsQuery();
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetReputationPerBrand")]
        public async Task<ActionResult> GetReputationPerBrand(ReputationPerBrandQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("PromotionMessge")]
        public async Task<ActionResult> PromotionMessge(PromotionMessgeCommand command)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            command.LastUpdatedBy = userId;
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin + "," + Role.User)]
        [HttpGet("GetPromotionMessge")]
        public async Task<ActionResult> GetPromotionMessge()
        {
            var promotionMessageQuery = new PromotionMessageQuery();
            var user = await _mediator.Send(promotionMessageQuery);
            return Ok(user);
        }
    }
}
