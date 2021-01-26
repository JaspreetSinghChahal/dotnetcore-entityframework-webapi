using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class PromoCodeBatchConfiguration : IEntityTypeConfiguration<PromoCodeBatch>
    {
        public void Configure(EntityTypeBuilder<PromoCodeBatch> builder)
        {
            builder.Property(e => e.BatchId).IsRequired();
            builder.Property(e => e.BatchName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.BrandId).IsRequired();
            builder.Property(e => e.ExpirationDateTime).IsRequired();
            builder.Property(e => e.NoOfPromoCodes).IsRequired();
            builder.Property(e => e.LoyaltyPoints).IsRequired();
            builder.Property(t => t.LastUpdatedOn)
                .IsRequired()
                .HasDefaultValueSql("GetDate()");
            builder.Property(e => e.LastUpdatedBy).IsRequired();

            builder.HasKey(e => new { e.BatchId });

            builder.HasCheckConstraint("ck_NoOfPromoCodes", "NoOfPromoCodes > 0");

            builder.HasMany<PromoCode>(s => s.PromoCodes)
                .WithOne(c => c.PromoCodeBatch)
                .HasForeignKey(b => b.BatchId);
        }
    }
}