using Autobot.Infrastructure.Auth.Model;
using Autobot.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Autobot.Infrastructure.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;
        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        public ClaimsIdentity GetClaimsIdentity(ApplicationUser user, IList<string> roles)
        {
            Claim[] claims = new[]
            {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
            claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claimsIdentity;
        }

        public Token GetJwtToken(ClaimsIdentity identity)
        {
            var jwt = new JwtSecurityToken(
               _jwtOptions.Issuer,
               _jwtOptions.Audience,
               identity.Claims,
               _jwtOptions.NotBefore,
               _jwtOptions.Expiration,
               _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new Token
            {
                Id = identity.Claims.Single(c => c.Type == ClaimTypes.Sid).Value,
                AuthToken = encodedJwt,
                ValidFor = (int)_jwtOptions.ValidFor.TotalSeconds
            };
        }
    }
}
