using MediatR;

namespace Autobot.Commands.Command
{
    public class AddBrandCommand : IRequest<string>
    {
        public string BrandName { get; set; }
    }
}
