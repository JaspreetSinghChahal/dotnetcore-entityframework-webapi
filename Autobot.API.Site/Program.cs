using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Autobot.API.Site
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var northwindContext = services.GetRequiredService<AutobotDbContext>();
            //        northwindContext.Database.Migrate();

            //        var identityContext = services.GetRequiredService<AutobotIdentityDbContext>();
            //        identityContext.Database.Migrate();

            //        var mediator = services.GetRequiredService<IMediator>();
            //        mediator.Send(new SeedInitialDataCommand(), CancellationToken.None).GetAwaiter().GetResult();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            //    }
            //}
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
