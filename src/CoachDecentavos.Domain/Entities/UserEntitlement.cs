using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class UserEntitlement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid? UserId { get; private set; }
    public string BuyerEmail { get; private set; } = string.Empty;
    public string? HotmartTransactionId { get; private set; }
    public EntitlementStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ActivatedAtUtc { get; private set; }

    public Product Product { get; private set; } = null!;
    public User? User { get; private set; }

    private UserEntitlement() { }

    public static UserEntitlement CreateFromHotmart(
        Guid productId,
        string buyerEmail,
        string hotmartTransactionId,
        Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(buyerEmail))
            throw new ArgumentException("Buyer email is required.", nameof(buyerEmail));
        if (string.IsNullOrWhiteSpace(hotmartTransactionId))
            throw new ArgumentException("Transaction id is required.", nameof(hotmartTransactionId));

        var normalizedEmail = buyerEmail.Trim().ToLowerInvariant();
        var status = userId.HasValue ? EntitlementStatus.Active : EntitlementStatus.Pending;

        return new UserEntitlement
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            UserId = userId,
            BuyerEmail = normalizedEmail,
            HotmartTransactionId = hotmartTransactionId.Trim(),
            Status = status,
            CreatedAtUtc = DateTime.UtcNow,
            ActivatedAtUtc = userId.HasValue ? DateTime.UtcNow : null,
        };
    }

    public void LinkToUser(Guid userId)
    {
        UserId = userId;
        Status = EntitlementStatus.Active;
        ActivatedAtUtc = DateTime.UtcNow;
    }

    public void Revoke()
    {
        Status = EntitlementStatus.Revoked;
    }
}
