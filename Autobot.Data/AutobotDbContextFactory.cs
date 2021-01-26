using Microsoft.EntityFrameworkCore;

namespace Autobot.Data
{
    public class AutobotDbContextFactory : DesignTimeDbContextFactoryBase<AutobotDbContext>
    {
        protected override AutobotDbContext CreateNewInstance(DbContextOptions<AutobotDbContext> options)
        {
            return new AutobotDbContext(options);
        }
    }
}
