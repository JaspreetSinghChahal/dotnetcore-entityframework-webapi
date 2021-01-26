using Autobot.Infrastructure.Identity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Commands.SystemSeedData
{
    public class SeedInitialDataCommandHandler : IRequestHandler<SeedInitialDataCommand>
    {
        private readonly IUserManagerRepository _repository;

        public SeedInitialDataCommandHandler(IUserManagerRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(SeedInitialDataCommand request, CancellationToken cancellationToken)
        {
            var seeder = new InitialDataSeeder(_repository);
            await seeder.SeedAllAsync();
            return Unit.Value;
        }
    }
}
