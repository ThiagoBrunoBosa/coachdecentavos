namespace CoachDecentavos.Application.Entitlements.Contracts;

public sealed record EntitlementDto(
    Guid Id,
    Guid ProductId,
    string ProductSlug,
    string ProductName,
    string Status,
    DateTime? ActivatedAtUtc);

public sealed record LinkPurchaseRequest(string BuyerEmail);

public sealed record HotmartWebhookRequest(
    string? Event,
    HotmartWebhookData? Data);

public sealed record HotmartWebhookData(
    HotmartBuyer? Buyer,
    HotmartPurchase? Purchase);

public sealed record HotmartBuyer(string? Email);

public sealed record HotmartPurchase(
    string? Transaction,
    HotmartProductRef? Product);

public sealed record HotmartProductRef(string? Id);
