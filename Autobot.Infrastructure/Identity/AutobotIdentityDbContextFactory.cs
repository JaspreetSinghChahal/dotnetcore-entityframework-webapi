using Microsoft.EntityFrameworkCore;

namespace Autobot.Infrastructure.Identity
{
    public class AutobotIdentityDbContextFactory : DesignTimeAutobotIdentityDbContextFactoryBase<AutobotIdentityDbContext>
    {
        protected override AutobotIdentityDbContext CreateNewInstance(DbContextOptions<AutobotIdentityDbContext> options)
        {
            return new AutobotIdentityDbContext(options);
        }
    }
}
