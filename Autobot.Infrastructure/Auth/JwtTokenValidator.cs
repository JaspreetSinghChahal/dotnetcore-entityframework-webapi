using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Autobot.Infrastructure.Auth
{
    public sealed class JwtTokenValidator : IJwtTokenValidator
    {
        public JwtTokenValidator()
        {
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, SymmetricSecurityKey signingKey)
        {
            var tokenParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = false
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, tokenParams, out _);
        }
    }
}
