using Autobot.Infrastructure.Auth;
using Autobot.Infrastructure.Identity;
using Autobot.Infrastructure.OpenXml;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Autobot.Infrastructure
{
    public static class InfrastructureDIModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserManagerRepository, UserManagerRepository>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();
            services.AddScoped<ITokenFactory, TokenFactory>();
            services.AddScoped<ISpreadsheetService, SpreadsheetService>();

            services.AddDbContext<AutobotIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AutobotDatabase")), ServiceLifetime.Transient);

            services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AutobotIdentityDbContext>();

            var identityBuilder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<AutobotIdentityDbContext>().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(1440));
            services.AddAuthentication();

            return services;
        }
    }
}
