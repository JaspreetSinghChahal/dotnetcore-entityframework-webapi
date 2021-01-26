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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public AuthController(IMediator mediator, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _mediator = mediator;
            _sharedLocalizer = sharedLocalizer;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginQuery query)
        {
            if (string.IsNullOrEmpty(query.UserName) || string.IsNullOrEmpty(query.Password))
            {
                return BadRequest(_sharedLocalizer["UsernamePasswordRequired"]);
            }
            if (query.IsTermsAndConditonsAccepted == false)
            {
                return BadRequest(_sharedLocalizer["AcceptTermsAndConditons"]);
            }
            var token = await _mediator.Send(query);
            if (token == null)
            {
                return BadRequest(_sharedLocalizer["InvalidUsername"]);
            }
            return Ok(token);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken(RefreshTokenCommand command)
        {
            if (string.IsNullOrEmpty(command.RefreshToken) || string.IsNullOrEmpty(command.AccessToken))
            {
                return BadRequest(_sharedLocalizer["AccessTokenAndRefreshTokenRequired"]);
            }

            var token = await _mediator.Send(command);
            if (token == null)
            {
                return BadRequest(_sharedLocalizer["InvalidToken"]);
            }
            return Ok(token);
        }

        [HttpGet("TermsAndConditions")]
        public async Task<ActionResult> TermsAndConditions()
        {
            TermsAndConditionsQuery query = new TermsAndConditionsQuery();
            var termsAndConditions = await _mediator.Send(query);
            return Ok(termsAndConditions);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("TermsAndConditions")]
        public async Task<ActionResult> TermsAndConditions(TermsAndConditionsCommand command)
        {
            if (string.IsNullOrEmpty(command.TermsAndConditionsText))
            {
                return BadRequest(_sharedLocalizer["TermsAndConditionsRequired"]);
            }

            var termsAndConditions = await _mediator.Send(command);
            return Ok(termsAndConditions);
        }
    }
}
