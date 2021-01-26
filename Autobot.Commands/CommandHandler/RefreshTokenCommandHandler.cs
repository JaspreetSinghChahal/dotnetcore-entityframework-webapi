using Autobot.Commands.Command;
using Autobot.Data.Interfaces;
using Autobot.Infrastructure.Auth;
using Autobot.Infrastructure.Auth.Model;
using Autobot.Infrastructure.Identity;
using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.CommandHandler
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Token>
    {
        private readonly IMapper _mapper;
        public readonly IJwtTokenValidator _jwtTokenValidator;
        private readonly IAutobotDbContext _context;
        private readonly IUserManagerRepository _repository;
        private readonly IJwtFactory _jwtFactory;

        public RefreshTokenCommandHandler(IMapper mapper, IJwtTokenValidator jwtTokenValidator, IAutobotDbContext context, IUserManagerRepository repository, IJwtFactory jwtFactory)
        {
            _mapper = mapper;
            _jwtTokenValidator = jwtTokenValidator;
            _context = context;
            _repository = repository;
            _jwtFactory = jwtFactory;
        }

        public async Task<Token> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Token token = new Token();
            string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH";
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            var cp = _jwtTokenValidator.GetPrincipalFromToken(request.AccessToken, signingKey);

            if (cp == null)
            {
                return null;
            }

            // invalid token/signing key was passed and we can't extract user claims
            var id = cp.Claims.Single(c => c.Type == ClaimTypes.Sid).Value;
            var refreshToken = _context.RefreshToken.Where(x => x.UserId == id).FirstOrDefault();
            if (refreshToken != null && refreshToken.Token == request.RefreshToken)
            {
                var user = await _repository.GetUserInfoById(id);
                // generate token
                var roles = await _repository.GetRoles(user);
                var claims = _jwtFactory.GetClaimsIdentity(user, roles);
                token = _jwtFactory.GetJwtToken(claims);
                token.RefreshToken = request.RefreshToken;
            }
            else
            {
                return null;
            }
            return token;
        }
    }
}