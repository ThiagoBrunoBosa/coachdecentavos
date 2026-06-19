namespace CoachDecentavos.Application.Consulting.Contracts;

public sealed record ConsultingPackageDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    int DurationMinutes,
    decimal Price,
    string Currency);

public sealed record AvailabilitySlotDto(Guid Id, DateTime StartsAtUtc, DateTime EndsAtUtc);

public sealed record CreateBookingRequest(Guid PackageId, Guid SlotId, string? Notes);

public sealed record BookingDto(
    Guid Id,
    string PackageName,
    DateTime StartsAtUtc,
    DateTime EndsAtUtc,
    string Status,
    string? MeetingUrl);
