using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Autobot.API.Site.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetUserId()
        {
            return this.User.Claims.First(i => i.Type == ClaimTypes.Sid).Value;
        }
    }
}
