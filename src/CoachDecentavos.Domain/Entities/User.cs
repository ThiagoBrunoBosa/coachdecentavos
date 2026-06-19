using CoachDecentavos.Domain.Auth;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public PreferredLocale PreferredLocale { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastLoginAtUtc { get; private set; }

    public ICollection<UserOAuthAccount> OAuthAccounts { get; private set; } = new List<UserOAuthAccount>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    private User() { }

    public static User CreateManual(string name, string email, string passwordHash, UserRole role, PreferredLocale locale)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = role,
            PreferredLocale = locale,
            IsBlocked = false,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public static User CreateFromSso(string name, string email, PreferredLocale locale, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = null,
            Role = role,
            PreferredLocale = locale,
            IsBlocked = false,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void MarkLogin()
    {
        LastLoginAtUtc = DateTime.UtcNow;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        PasswordHash = passwordHash;
    }

    public void UpdateProfile(string name, PreferredLocale locale)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();
        PreferredLocale = locale;
    }
}