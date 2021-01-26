using Autobot.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autobot.Data
{
    public static class RepositoryDIModule
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AutobotDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AutobotDatabase")));

            services.AddScoped<IAutobotDbContext>(provider => provider.GetService<AutobotDbContext>());

            return services;
        }
    }
}
