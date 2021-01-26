using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class PromotionMessageConfiguration : IEntityTypeConfiguration<PromotionMessage>
    {
        public void Configure(EntityTypeBuilder<PromotionMessage> builder)
        {
            builder.Property(e => e.Id).UseIdentityColumn(1, 1);
            builder.Property(e => e.PromotionText).IsRequired();
            builder.Property(e => e.PromotionFileName).IsRequired();
            builder.Property(t => t.LastUpdatedOn)
                .IsRequired()
                .HasDefaultValueSql("GetDate()");
            builder.Property(e => e.LastUpdatedBy).IsRequired();
            builder.HasKey(e => new { e.Id });
        }
    }
}