using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.Property(e => e.Id).UseIdentityColumn(1, 1);
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Token).IsRequired();
            builder.HasKey(e => new { e.Id });
        }
    }
}
