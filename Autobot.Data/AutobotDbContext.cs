using Autobot.Data.Interfaces;
using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Autobot.Data
{
    public class AutobotDbContext : DbContext, IAutobotDbContext
    {
        public AutobotDbContext(DbContextOptions<AutobotDbContext> options)
            : base(options)
        {
        }

        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<UserScan> UserScans { get; set; }
        public DbSet<PromoCodeBatch> PromoCodeBatch { get; set; }
        public DbSet<PromotionMessage> PromotionMessage { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public DbSet<UserPointReset> UserPointReset { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutobotDbContext).Assembly);
        }
    }
}
