namespace CoachDecentavos.Domain.Entities;

public class ConsultingPackage
{
    public Guid Id { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int DurationMinutes { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public bool IsPublished { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private ConsultingPackage() { }

    public static ConsultingPackage Create(
        string slug,
        string name,
        int durationMinutes,
        decimal price,
        string? description = null,
        string currency = "BRL")
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug is required.", nameof(slug));
        if (durationMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationMinutes));

        return new ConsultingPackage
        {
            Id = Guid.NewGuid(),
            Slug = slug.Trim().ToLowerInvariant(),
            Name = name.Trim(),
            Description = description?.Trim(),
            DurationMinutes = durationMinutes,
            Price = price,
            Currency = currency.Trim().ToUpperInvariant(),
            IsPublished = false,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void Publish() => IsPublished = true;
}
