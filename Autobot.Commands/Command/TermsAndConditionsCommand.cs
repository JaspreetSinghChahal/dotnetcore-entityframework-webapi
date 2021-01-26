using MediatR;

namespace Autobot.Commands.Command
{
    public class TermsAndConditionsCommand : IRequest<bool>
    {
        public string TermsAndConditionsText { get; set; }
    }
}