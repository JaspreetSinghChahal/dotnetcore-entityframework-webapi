using Autobot.Infrastructure.Auth.Model;
using Autobot.Infrastructure.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace Autobot.Infrastructure.Auth
{
    public interface IJwtFactory
    {
        ClaimsIdentity GetClaimsIdentity(ApplicationUser user, IList<string> roles);
        Token GetJwtToken(ClaimsIdentity identity);
    }
}