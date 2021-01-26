using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Autobot.Queries
{
    public static class QueryDIModule
    {
        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
