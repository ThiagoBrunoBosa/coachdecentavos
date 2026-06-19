using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Entitlements.Contracts;
using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Entitlements;

public sealed class ProcessHotmartWebhookService
{
    private readonly IProductRepository _products;
    private readonly IEntitlementRepository _entitlements;
    private readonly IUserRepository _users;

    public ProcessHotmartWebhookService(
        IProductRepository products,
        IEntitlementRepository entitlements,
        IUserRepository users)
    {
        _products = products;
        _entitlements = entitlements;
        _users = users;
    }

    public async Task<bool> ExecuteAsync(HotmartWebhookRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Data?.Buyer?.Email?.Trim().ToLowerInvariant();
        var transactionId = request.Data?.Purchase?.Transaction?.Trim();
        var productId = request.Data?.Purchase?.Product?.Id?.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(productId))
            return false;

        var existing = await _entitlements.GetByTransactionIdAsync(transactionId, cancellationToken);
        if (existing is not null)
            return true;

        var product = await _products.GetByHotmartProductIdAsync(productId, cancellationToken);
        if (product is null)
            return false;

        var user = await _users.GetByEmailAsync(email, cancellationToken);
        var entitlement = UserEntitlement.CreateFromHotmart(product.Id, email, transactionId, user?.Id);
        await _entitlements.AddAsync(entitlement, cancellationToken);
        await _entitlements.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public sealed class ListEntitlementsService
{
    private readonly IEntitlementRepository _entitlements;

    public ListEntitlementsService(IEntitlementRepository entitlements) => _entitlements = entitlements;

    public async Task<IReadOnlyList<EntitlementDto>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var items = await _entitlements.ListForUserAsync(userId, cancellationToken);
        return items.Select(x => new EntitlementDto(
            x.Id,
            x.ProductId,
            x.Product.Slug,
            x.Product.Name,
            x.Status.ToString(),
            x.ActivatedAtUtc)).ToList();
    }
}

public sealed class LinkEntitlementService
{
    private readonly IEntitlementRepository _entitlements;
    private readonly IUserRepository _users;

    public LinkEntitlementService(IEntitlementRepository entitlements, IUserRepository users)
    {
        _entitlements = entitlements;
        _users = users;
    }

    public async Task<int> ExecuteAsync(Guid userId, string buyerEmail, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        var normalized = buyerEmail.Trim().ToLowerInvariant();
        if (normalized != user.Email)
            throw new InvalidOperationException("Buyer email must match the purchase email.");

        var pending = await _entitlements.ListPendingByEmailAsync(normalized, cancellationToken);
        foreach (var item in pending)
            item.LinkToUser(userId);

        if (pending.Count > 0)
            await _entitlements.SaveChangesAsync(cancellationToken);

        return pending.Count;
    }
}
