using CoachDecentavos.Application.Common.Interfaces;

namespace CoachDecentavos.Application.Shorts;

public sealed record YouTubeShortDto(string VideoId, string Title, string? ThumbnailUrl);

public sealed class ListYouTubeShortsService
{
    private readonly IYouTubeShortRepository _repository;

    public ListYouTubeShortsService(IYouTubeShortRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<YouTubeShortDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var items = await _repository.ListPublishedAsync(cancellationToken);
        return Map(items);
    }

    public async Task<IReadOnlyList<YouTubeShortDto>> ExecuteLatestAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        var items = await _repository.ListLatestPublishedAsync(limit, cancellationToken);
        return Map(items);
    }

    private static IReadOnlyList<YouTubeShortDto> Map(IReadOnlyList<Domain.Entities.YouTubeShort> items)
        => items.Select(x => new YouTubeShortDto(x.VideoId, x.Title, x.ThumbnailUrl)).ToList();
}
