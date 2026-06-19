namespace CoachDecentavos.Domain.Entities;

public class YouTubeShort
{
    public Guid Id { get; private set; }
    public string VideoId { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string? ThumbnailUrl { get; private set; }
    public int SortOrder { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private YouTubeShort() { }

    public static YouTubeShort Create(string videoId, string title, int sortOrder, string? thumbnailUrl = null)
    {
        if (string.IsNullOrWhiteSpace(videoId))
            throw new ArgumentException("Video id is required.", nameof(videoId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        return new YouTubeShort
        {
            Id = Guid.NewGuid(),
            VideoId = videoId.Trim(),
            Title = title.Trim(),
            ThumbnailUrl = thumbnailUrl,
            SortOrder = sortOrder,
            IsPublished = true,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}