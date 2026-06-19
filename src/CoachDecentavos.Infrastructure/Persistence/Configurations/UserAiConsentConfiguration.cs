using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class UserAiConsentConfiguration : IEntityTypeConfiguration<UserAiConsent>
{
    public void Configure(EntityTypeBuilder<UserAiConsent> builder)
    {
        builder.ToTable("user_ai_consents");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.UserId).IsUnique();
        builder.Property(x => x.DisclaimerVersion).HasMaxLength(32).IsRequired();
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
