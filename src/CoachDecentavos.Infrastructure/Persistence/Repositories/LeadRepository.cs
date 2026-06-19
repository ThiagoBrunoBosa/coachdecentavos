using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class LeadRepository : ILeadRepository
{
    private readonly AppDbContext _dbContext;

    public LeadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(LeadInterest lead, CancellationToken cancellationToken = default)
        => await _dbContext.LeadInterests.AddAsync(lead, cancellationToken);

    public async Task<IReadOnlyList<LeadInterest>> ListAsync(CancellationToken cancellationToken = default)
        => await _dbContext.LeadInterests
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}