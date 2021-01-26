using MediatR;

namespace Autobot.Queries.Query
{
    public class ForgotPasswordQuery : IRequest<string>
    {
        public string Email { get; set; }
    }
}