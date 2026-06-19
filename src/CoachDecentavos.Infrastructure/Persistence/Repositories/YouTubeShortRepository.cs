using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class YouTubeShortRepository : IYouTubeShortRepository
{
    private readonly AppDbContext _db;

    public YouTubeShortRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<YouTubeShort>> ListPublishedAsync(CancellationToken cancellationToken = default)
        => await _db.YouTubeShorts
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<YouTubeShort>> ListLatestPublishedAsync(
        int limit,
        CancellationToken cancellationToken = default)
        => await _db.YouTubeShorts
            .Where(x => x.IsPublished)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(Math.Clamp(limit, 1, 12))
            .ToListAsync(cancellationToken);
}
