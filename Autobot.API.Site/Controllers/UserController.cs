using Autobot.API.Site.Common;
using Autobot.API.Site.Resources;
using Autobot.Commands.Command;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Autobot.API.Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public UserController(IMediator mediator, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _mediator = mediator;
            _sharedLocalizer = sharedLocalizer;
        }

        /// <summary>
        /// Get list of all users
        /// Features supported : 1. Server side pagination.
        ///                      2. Search 
        ///                      3. Sort
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetUsers")]
        public async Task<ActionResult> GetUsers(GetUserListQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Details of a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("GetUserInfo/{id}")]
        public async Task<ActionResult> GetUserInfo(string id)
        {
            var query = new GetUserInfoByIdQuery()
            {
                Id = id
            };
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Loyalty points points of a user grouped by brands
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetUsersReputationPerBrand")]
        public async Task<ActionResult> GetUsersLoyaltyPointsPerBrand(GetUsersReputationPerBrandQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// All the promocodes scanned by a user
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetUserScanActivity")]
        public async Task<ActionResult> GetUserScanActivity(UserScanActivityQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        /// <summary>
        /// Update user command
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UpdateUserCommand command)
        {
            var updateduserId = await _mediator.Send(command);
            if (updateduserId == null)
            {
                return NotFound(_sharedLocalizer["UserNotExists"]);
            }
            return Ok(updateduserId);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("DeleteUsers")]
        public async Task<ActionResult> DeleteUsers(DeleteUsersCommand query)
        {
            var data = await _mediator.Send(query);
            if (data == "failure")
            {
                return BadRequest("No Data found");
            }
            return Ok(data);
        }

        /// <summary>
        /// Total points till date
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        [HttpGet("GetUserTotalPoints")]
        public async Task<ActionResult> GetUserTotalPoints()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            var query = new GetTotalPointsQuery()
            {
                UserId = userId
            };
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Reset user points
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("UserResetPoints")]
        public async Task<ActionResult> UserResetPoints(UserPointResetCommand command)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            command.LastUpdatedByUserId = userId;
            var user = await _mediator.Send(command);
            return Ok(user);
        }

        /// <summary>
        /// Get list of point reset done for a user
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("UserResetPointData")]
        public async Task<ActionResult> UserResetPointData(UserResetPointQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }
    }
}