using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class ConsultingPackageConfiguration : IEntityTypeConfiguration<ConsultingPackage>
{
    public void Configure(EntityTypeBuilder<ConsultingPackage> builder)
    {
        builder.ToTable("consulting_packages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Slug).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(300).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(4000);
        builder.Property(x => x.Price).HasPrecision(18, 2);
        builder.Property(x => x.Currency).HasMaxLength(3).IsRequired();
    }
}
