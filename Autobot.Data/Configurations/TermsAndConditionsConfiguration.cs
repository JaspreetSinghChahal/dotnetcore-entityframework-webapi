using Autobot.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autobot.Data.Configurations
{
    public class TermsAndConditionsConfiguration : IEntityTypeConfiguration<TermsAndConditions>
    {
        public void Configure(EntityTypeBuilder<TermsAndConditions> builder)
        {
            builder.Property(e => e.Id).UseIdentityColumn(0, 1);
            builder.Property(e => e.TermsAndConditionsText).IsRequired();
            builder.Property(e => e.LastUpdatedOn).IsRequired();
        }
    }
}
