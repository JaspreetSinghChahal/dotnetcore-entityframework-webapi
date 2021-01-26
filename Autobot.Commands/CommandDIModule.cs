using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Autobot.Commands
{
    public static class CommandDIModule
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
