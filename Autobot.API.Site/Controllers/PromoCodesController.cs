using Autobot.API.Site.Common;
using Autobot.Commands.Command;
using Autobot.Models;
using Autobot.Queries.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autobot.API.Site.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromoCodesController : BaseController
    {
        private readonly IMediator _mediator;

        public PromoCodesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all promocodes in the system
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetPromoCodes")]
        public async Task<ActionResult> GetPromoCodes(GetPromoCodesListQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        /// <summary>
        /// Details of a promocode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("GetPromoCodeInfo/{id}")]
        public async Task<ActionResult> GetPromoCodeInfo(long id)
        {
            var query = new GetPromocodeInfoByIdQuery()
            {
                Id = id
            };
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        /// <summary>
        /// Add a promocode
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("AddPromoCode")]
        public async Task<IActionResult> AddPromoCode(AddPromoCodeCommand command)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }
            command.UserId = userId;
            var id = await _mediator.Send(command);
            return Ok(id);
        }


        /// <summary>
        /// Details of a promocode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetPromoCodeScanActivity")]
        public async Task<ActionResult> GetPromoCodeScanActivity(GetPromoCodeScanActivityByIdQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }

        /// <summary>
        /// Get all batch promo codes
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetBatchPromoCodes")]
        public async Task<ActionResult> GetBatchPromoCodes(GetPromocodeBatchListQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Get batch info
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("GetPromoCodeBatchInfo/{id}")]
        public async Task<ActionResult> GetPromoCodeBatchInfo(string id)
        {
            var query = new GetPromocodeBatchInfoByIdQuery()
            {
                Id = id
            };
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Details of a promocode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("GetPromoCodeBatchScanActivity")]
        public async Task<ActionResult> GetPromoCodeBatchScanActivity(GetPromoCodeBatchScanActivityByIdQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }


        /// <summary>
        /// Delete promocodes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("DeletePromoCodes")]
        public async Task<ActionResult> DeletePromoCodes(DeletePromoCodesCommand query)
        {
            var data = await _mediator.Send(query);
            if (data == "failure")
            {
                return BadRequest("No Data found");
            }
            return Ok(data);
        }


        /// <summary>
        /// Delete promocode batch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("DeletePromoCodeBatch")]
        public async Task<ActionResult> DeletePromoCodeBatch(DeletePromoCodeBatchCommand query)
        {
            var data = await _mediator.Send(query);
            if (data == "failure")
            {
                return BadRequest("No Data found");
            }
            return Ok(data);
        }


        /// <summary>
        /// Download zip file of Qr codes.      
        /// Zip has png files of Qr codes
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("DownloadQRCode")]
        public async Task<ActionResult> DownloadQRCode(DownloadQRCodeQuery query)
        {
            var bytes = await _mediator.Send(query);
            return File(bytes, "application/zip");
        }


        /// <summary>
        /// scan promocode
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize(Roles = Role.User)]
        [HttpPost("ScanPromoCode")]
        public async Task<IActionResult> ScanPromoCode(Scan scanId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return BadRequest("User Id not found");
            }

            var scannedText = scanId.Id.Split("_");
            if (scannedText.Length != 2)
            {
                return BadRequest("Invalid scan");
            }

            var command = new ScanPromocodeCommand
            {
                UserId = userId,
                PromoCodeNumber = long.Parse(scannedText[0]),
                PromoCodeId = scannedText[1]
            };

            var result = await _mediator.Send(command);
            if (result.StatusCode == "400")
            {
                return BadRequest(result.Response);
            }
            else
            {
                return Ok(result.Response);
            }
        }
    }
}
