using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoachDecentavos.Infrastructure.Persistence.Configurations;

public class UserOAuthAccountConfiguration : IEntityTypeConfiguration<UserOAuthAccount>
{
    public void Configure(EntityTypeBuilder<UserOAuthAccount> builder)
    {
        builder.ToTable("user_oauth_accounts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Provider).HasMaxLength(64).IsRequired();
        builder.Property(x => x.ProviderUserId).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => new { x.Provider, x.ProviderUserId }).IsUnique();
    }
}