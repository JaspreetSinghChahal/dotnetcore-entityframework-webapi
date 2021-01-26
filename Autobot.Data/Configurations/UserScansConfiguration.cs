using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class UserScansConfiguration : IEntityTypeConfiguration<UserScan>
    {
        public void Configure(EntityTypeBuilder<UserScan> builder)
        {
            builder.Property(e => e.UserScanId).UseIdentityColumn(0, 1);
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.PromoCodeNumber).IsRequired();
            builder.Property(e => e.IsSuccess).IsRequired().HasDefaultValue(false);
        }
    }
}

