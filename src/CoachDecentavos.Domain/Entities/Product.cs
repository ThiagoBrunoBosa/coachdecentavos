using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ProductType Type { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public bool IsPublished { get; private set; }
    public string? HotmartProductId { get; private set; }
    public string? HotmartCheckoutUrl { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    private Product() { }

    public static Product Create(string slug, string name, ProductType type, decimal price, string currency = "BRL")
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug is required.", nameof(slug));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price));

        return new Product
        {
            Id = Guid.NewGuid(),
            Slug = slug.Trim().ToLowerInvariant(),
            Name = name.Trim(),
            Type = type,
            Price = price,
            Currency = currency.Trim().ToUpperInvariant(),
            IsPublished = false,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void ConfigureHotmart(string? productId, string? checkoutUrl)
    {
        HotmartProductId = string.IsNullOrWhiteSpace(productId) ? null : productId.Trim();
        HotmartCheckoutUrl = string.IsNullOrWhiteSpace(checkoutUrl) ? null : checkoutUrl.Trim();
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Publish()
    {
        IsPublished = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description, decimal price)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name.Trim();
        Description = description?.Trim();
        if (price >= 0)
            Price = price;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}