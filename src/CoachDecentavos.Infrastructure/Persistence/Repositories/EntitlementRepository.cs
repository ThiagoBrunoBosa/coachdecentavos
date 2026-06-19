using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class EntitlementRepository : IEntitlementRepository
{
    private readonly AppDbContext _db;

    public EntitlementRepository(AppDbContext db) => _db = db;

    public Task<UserEntitlement?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
        => _db.UserEntitlements.FirstOrDefaultAsync(x => x.HotmartTransactionId == transactionId, cancellationToken);

    public async Task<IReadOnlyList<UserEntitlement>> ListForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _db.UserEntitlements
            .Include(x => x.Product)
            .Where(x => x.UserId == userId && x.Status == EntitlementStatus.Active)
            .OrderByDescending(x => x.ActivatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<UserEntitlement>> ListPendingByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _db.UserEntitlements
            .Include(x => x.Product)
            .Where(x => x.BuyerEmail == email && x.Status == EntitlementStatus.Pending)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(UserEntitlement entitlement, CancellationToken cancellationToken = default)
        => await _db.UserEntitlements.AddAsync(entitlement, cancellationToken);

    public Task UpdateAsync(UserEntitlement entitlement, CancellationToken cancellationToken = default)
    {
        _db.UserEntitlements.Update(entitlement);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
