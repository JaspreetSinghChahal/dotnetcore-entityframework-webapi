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
    public class BrandController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public BrandController(IMediator mediator, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _mediator = mediator;
            _sharedLocalizer = sharedLocalizer;
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetBrands")]
        public async Task<ActionResult> GetBrands(BrandQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        [Authorize(Roles = Role.Admin)]
        [HttpPost("AddBrand")]
        public async Task<ActionResult> AddBrand(AddBrandCommand command)
        {
            var brand = await _mediator.Send(command);
            if (brand == null)
            {
                return BadRequest(_sharedLocalizer["BrandAlreadyExists"]);
            }
            return Ok(brand);
        }
    }
}
