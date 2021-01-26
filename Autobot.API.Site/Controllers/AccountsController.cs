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
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public AccountsController(IMediator mediator, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _mediator = mediator;
            _sharedLocalizer = sharedLocalizer;
        }

        /// <summary>
        /// Register new user
        /// Phone number acts as username
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            var getUserQuery = new GetUserByUserNameQuery
            {
                UserName = command.PhoneNumber
            };
            var user = await _mediator.Send(getUserQuery);
            if (user != null)
            {
                return BadRequest(_sharedLocalizer["PhoneNumberExists"]);
            }
            var id = await _mediator.Send(command);
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(_sharedLocalizer["UserCreationFailure"]);
            }
            return Ok(id);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(res);
        }

        // <summary>
        /// Get all roles existing in system
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        [HttpGet("AllRoles")]
        public async Task<ActionResult> AllRoles()
        {
            GetAllRolesQuery query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query);
            if (roles == null)
            {
                return BadRequest(_sharedLocalizer["NoRoleExists"]);
            }
            return Ok(roles);
        }
    }
}