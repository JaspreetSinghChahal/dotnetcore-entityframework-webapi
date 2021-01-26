using Autobot.API.Site.Common;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autobot.API.Site.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all promocodes in the system
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>

        [HttpGet("GetUserReport")]
        public async Task<ActionResult> GetUserReport()
        {
            var query = new DownloadUserDataInExcelQuery();
            var userBytes = await _mediator.Send(query);
            return File(userBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpGet("GetUserReputation")]
        public async Task<ActionResult> GetUserReputation()
        {
            var query = new DownloadUserReputationByBrandInExcelQuery();
            var userBytes = await _mediator.Send(query);
            return File(userBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpGet("GetPromocodes")]
        public async Task<ActionResult> GetPromocodes()
        {
            var query = new DownloadPromocodeDataInExcelQuery();
            var userBytes = await _mediator.Send(query);
            return File(userBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
