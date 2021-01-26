using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;


namespace Autobot.Data.Interfaces
{
    public interface IAutobotDbContext
    {
        DbSet<PromoCode> PromoCodes { get; set; }
        DbSet<UserScan> UserScans { get; set; }
        DbSet<PromoCodeBatch> PromoCodeBatch { get; set; }
        DbSet<PromotionMessage> PromotionMessage { get; set; }
        DbSet<Brand> Brand { get; set; }
        DbSet<RefreshToken> RefreshToken { get; set; }
        DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        DbSet<UserPointReset> UserPointReset { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
