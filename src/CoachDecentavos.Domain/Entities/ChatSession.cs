namespace CoachDecentavos.Domain.Entities;

public class ChatSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? ProductId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public User User { get; private set; } = null!;
    public ICollection<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

    private ChatSession() { }

    public static ChatSession Create(Guid userId, Guid? productId = null)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = productId,
            CreatedAtUtc = DateTime.UtcNow,
        };
}
