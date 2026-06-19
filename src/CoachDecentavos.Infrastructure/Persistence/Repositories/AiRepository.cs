using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class AiRepository : IAiRepository
{
    private readonly AppDbContext _db;

    public AiRepository(AppDbContext db) => _db = db;

    public Task<bool> HasConsentAsync(Guid userId, CancellationToken cancellationToken = default)
        => _db.UserAiConsents.AnyAsync(x => x.UserId == userId, cancellationToken);

    public async Task AddConsentAsync(UserAiConsent consent, CancellationToken cancellationToken = default)
        => await _db.UserAiConsents.AddAsync(consent, cancellationToken);

    public async Task<ChatSession> GetOrCreateSessionAsync(Guid userId, Guid? productId, CancellationToken cancellationToken = default)
    {
        var existing = await _db.ChatSessions
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId, cancellationToken);

        if (existing is not null)
            return existing;

        var session = ChatSession.Create(userId, productId);
        await _db.ChatSessions.AddAsync(session, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task AddMessageAsync(ChatMessage message, CancellationToken cancellationToken = default)
        => await _db.ChatMessages.AddAsync(message, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);
}
