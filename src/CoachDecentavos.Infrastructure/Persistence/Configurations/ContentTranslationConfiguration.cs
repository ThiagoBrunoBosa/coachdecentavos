using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class ContentTranslationConfiguration : IEntityTypeConfiguration<ContentTranslation>
{
    public void Configure(EntityTypeBuilder<ContentTranslation> builder)
    {
        builder.ToTable("content_translations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EntityType).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.Locale).HasConversion<string>().HasMaxLength(16);
        builder.Property(x => x.FieldKey).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Value).IsRequired();
        builder.HasIndex(x => new { x.EntityType, x.EntityId, x.Locale, x.FieldKey }).IsUnique();
    }
}