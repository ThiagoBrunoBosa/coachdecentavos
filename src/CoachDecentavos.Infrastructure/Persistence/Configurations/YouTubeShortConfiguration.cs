using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class YouTubeShortConfiguration : IEntityTypeConfiguration<YouTubeShort>
{
    public void Configure(EntityTypeBuilder<YouTubeShort> builder)
    {
        builder.ToTable("youtube_shorts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.VideoId).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.VideoId).IsUnique();
        builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
        builder.Property(x => x.ThumbnailUrl).HasMaxLength(500);
    }
}