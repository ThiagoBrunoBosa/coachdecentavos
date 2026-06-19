namespace CoachDecentavos.Application.Consulting.Contracts;

public sealed record AdminBookingDto(
    Guid Id,
    string UserName,
    string UserEmail,
    string PackageName,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    string Status);

public sealed record ConfirmBookingRequest(string? MeetingUrl);

public sealed record CreateAvailabilitySlotRequest(DateTime StartsAtUtc, DateTime EndsAtUtc);

public sealed record AdminAvailabilitySlotDto(
    Guid Id,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    bool IsBlocked,
    bool IsBooked);
