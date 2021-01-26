using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class PromoCodesConfiguration : IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.Property(e => e.PromoCodeNumber).UseIdentityColumn(1000, 1);
            builder.Property(e => e.PromoCodeId).IsRequired();
            builder.Property(e => e.BatchId).IsRequired();
            builder.Property(t => t.LastUpdatedOn)
                .IsRequired()
                .HasDefaultValueSql("GetDate()");
            builder.Property(e => e.LastUpdatedBy).IsRequired();

            builder.HasKey(e => new { e.PromoCodeNumber });

            builder.HasMany<UserScan>(s => s.UserScans)
                .WithOne(c => c.PromoCode)
                .HasForeignKey(b => b.PromoCodeNumber);
        }
    }
}
