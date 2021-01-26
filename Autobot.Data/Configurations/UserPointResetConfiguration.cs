using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class UserPointResetConfiguration : IEntityTypeConfiguration<UserPointReset>
    {
        public void Configure(EntityTypeBuilder<UserPointReset> builder)
        {
            builder.Property(e => e.UserPointResetId).UseIdentityColumn(0, 1);
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.PointsReset).IsRequired();
            builder.Property(e => e.ResetDateTime).IsRequired();
            builder.Property(e => e.LastUpdatedByUserId).IsRequired();
            builder.HasKey(e => new { e.UserPointResetId });
        }
    }
}