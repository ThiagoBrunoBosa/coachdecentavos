using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Application.Common.Interfaces;

public interface IEntitlementRepository
{
    Task<UserEntitlement?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserEntitlement>> ListForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserEntitlement>> ListPendingByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(UserEntitlement entitlement, CancellationToken cancellationToken = default);
    Task UpdateAsync(UserEntitlement entitlement, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IConsultingRepository
{
    Task<IReadOnlyList<ConsultingPackage>> ListPublishedPackagesAsync(CancellationToken cancellationToken = default);
    Task<ConsultingPackage?> GetPackageByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AvailabilitySlot>> ListOpenSlotsAsync(DateTime fromUtc, CancellationToken cancellationToken = default);
    Task<AvailabilitySlot?> GetSlotByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsSlotBookedAsync(Guid slotId, CancellationToken cancellationToken = default);
    Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> ListBookingsForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Booking>> ListAllBookingsAsync(CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Booking?> GetBookingForUserAsync(Guid userId, Guid bookingId, CancellationToken cancellationToken = default);
    Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AvailabilitySlot>> ListAllFutureSlotsAsync(DateTime fromUtc, CancellationToken cancellationToken = default);
    Task AddSlotAsync(AvailabilitySlot slot, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IAiRepository
{
    Task<bool> HasConsentAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddConsentAsync(UserAiConsent consent, CancellationToken cancellationToken = default);
    Task<ChatSession> GetOrCreateSessionAsync(Guid userId, Guid? productId, CancellationToken cancellationToken = default);
    Task AddMessageAsync(ChatMessage message, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IGroqChatClient
{
    Task<string> AskAsync(string systemPrompt, string userMessage, int maxTokens, CancellationToken cancellationToken = default);
}

public interface IRateLimitService
{
    bool TryAcquire(string key, int maxRequests, TimeSpan window);
}
