namespace CoachDecentavos.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; private set; }
    public Guid ChatSessionId { get; private set; }
    public string Role { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }

    public ChatSession ChatSession { get; private set; } = null!;

    private ChatMessage() { }

    public static ChatMessage Create(Guid chatSessionId, string role, string content)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.", nameof(role));
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content is required.", nameof(content));

        return new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatSessionId = chatSessionId,
            Role = role.Trim().ToLowerInvariant(),
            Content = content.Trim(),
            CreatedAtUtc = DateTime.UtcNow,
        };
    }
}
