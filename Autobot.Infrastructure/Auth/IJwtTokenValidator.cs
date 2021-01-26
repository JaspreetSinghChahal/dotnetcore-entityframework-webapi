using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Autobot.Infrastructure.Auth
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, SymmetricSecurityKey signingKey);
    }
}