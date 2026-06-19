namespace CoachDecentavos.Domain.Entities;

public class UserOAuthAccount
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public string Provider { get; private set; } = string.Empty;
    public string ProviderUserId { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    private UserOAuthAccount() { }

    public static UserOAuthAccount Create(Guid userId, string provider, string providerUserId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User id is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("Provider is required.", nameof(provider));
        if (string.IsNullOrWhiteSpace(providerUserId))
            throw new ArgumentException("Provider user id is required.", nameof(providerUserId));

        return new UserOAuthAccount
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Provider = provider.Trim().ToLowerInvariant(),
            ProviderUserId = providerUserId.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}