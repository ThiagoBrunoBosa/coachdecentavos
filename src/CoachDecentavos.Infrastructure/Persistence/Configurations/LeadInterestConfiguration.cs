using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class LeadInterestConfiguration : IEntityTypeConfiguration<LeadInterest>
{
    public void Configure(EntityTypeBuilder<LeadInterest> builder)
    {
        builder.ToTable("lead_interests");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(200);
        builder.Property(x => x.Phone).HasMaxLength(50);
        builder.Property(x => x.Source).HasMaxLength(100);
        builder.Property(x => x.Message).HasMaxLength(4000);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32);
    }
}