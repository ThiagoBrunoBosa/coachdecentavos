using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Common.Interfaces;

public interface IYouTubeShortRepository
{
    Task<IReadOnlyList<YouTubeShort>> ListPublishedAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<YouTubeShort>> ListLatestPublishedAsync(
        int limit,
        CancellationToken cancellationToken = default);
}
