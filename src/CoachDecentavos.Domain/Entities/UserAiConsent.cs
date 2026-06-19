namespace CoachDecentavos.Domain.Entities;

public class UserAiConsent
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string DisclaimerVersion { get; private set; } = string.Empty;
    public DateTime AcceptedAtUtc { get; private set; }

    public User User { get; private set; } = null!;

    private UserAiConsent() { }

    public static UserAiConsent Create(Guid userId, string disclaimerVersion)
    {
        if (string.IsNullOrWhiteSpace(disclaimerVersion))
            throw new ArgumentException("Disclaimer version is required.", nameof(disclaimerVersion));

        return new UserAiConsent
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DisclaimerVersion = disclaimerVersion.Trim(),
            AcceptedAtUtc = DateTime.UtcNow,
        };
    }
}
