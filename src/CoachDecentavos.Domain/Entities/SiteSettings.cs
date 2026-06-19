namespace CoachDecentavos.Domain.Entities;

public class SiteSettings
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public DateTime UpdatedAtUtc { get; private set; }

    private SiteSettings() { }

    public static SiteSettings Create(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key is required.", nameof(key));

        return new SiteSettings
        {
            Id = Guid.NewGuid(),
            Key = key.Trim(),
            Value = value ?? string.Empty,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }
}