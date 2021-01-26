using Autobot.Infrastructure.Auth.Model;
using MediatR;

namespace Autobot.Queries.Query
{
    public class LoginQuery : IRequest<Token>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsTermsAndConditonsAccepted { get; set; }
    }
}
