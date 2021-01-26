using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autobot.Infrastructure.Identity
{
    public class AutobotIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AutobotIdentityDbContext(DbContextOptions<AutobotIdentityDbContext> options) : base(options)
        {
        }
    }
}
