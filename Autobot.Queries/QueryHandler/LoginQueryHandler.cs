using Autobot.Data.Interfaces;
using Autobot.Infrastructure.Auth;
using Autobot.Infrastructure.Auth.Model;
using Autobot.Infrastructure.Identity;
using Autobot.Models.Entities;
using Autobot.Queries.Query;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Queries.QueryHandler
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Token>
    {
        private readonly IUserManagerRepository _repository;
        private readonly IAutobotDbContext _context;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public LoginQueryHandler(IUserManagerRepository repository, IJwtFactory jwtFactory, ITokenFactory tokenFactory, IAutobotDbContext context)
        {
            _repository = repository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _context = context;
        }

        /// <summary>
        /// Get user details by username
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>user detail</returns>
        public async Task<Token> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserDetailByUserName(request.UserName);
            if (user == null)
            {
                return null;
            }
            var validPassword = await _repository.CheckPassword(user, request.Password);
            if (validPassword == false)
            {
                return null;
            }

            // generate token
            var roles = await _repository.GetRoles(user);
            var claims = _jwtFactory.GetClaimsIdentity(user, roles);
            var token = _jwtFactory.GetJwtToken(claims);

            // Get refreshToken
            var refreshToken = _tokenFactory.GenerateToken();
            // Get token for user
            var existingToken = _context.RefreshToken.Where(x => x.UserId == user.Id).FirstOrDefault();
            if (existingToken == null)
            {
                _context.RefreshToken.Add(
                    new RefreshToken()
                    {
                        Token = refreshToken,
                        Expires = null,
                        UserId = user.Id,
                        RemoteIpAddress = null
                    });
            }
            else
            {
                existingToken.Token = refreshToken;
                _context.RefreshToken.Update(existingToken);
            }
            // Update terms and condition
            user.IsTermsAndConditonsAccepted = true;
            user.TermsAndConditonsAcceptedOn = DateTime.Now;
            await _repository.UpdateUserAsync(user);

            await _context.SaveChangesAsync(cancellationToken);

            token.RefreshToken = refreshToken;
            return token;
        }
    }
}
