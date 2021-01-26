using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(e => e.BrandId).UseIdentityColumn(1, 1);
            builder.Property(e => e.BrandName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.BrandName).IsRequired().HasMaxLength(100);
            builder.HasKey(e => new { e.BrandId });

            builder.HasMany<PromoCodeBatch>(s => s.PromoCodeBatch)
               .WithOne(c => c.Brand)
               .HasForeignKey(b => b.BrandId);
        }
    }
}