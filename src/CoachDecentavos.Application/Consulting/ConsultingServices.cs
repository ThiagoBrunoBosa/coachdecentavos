using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Consulting.Contracts;
using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Consulting;

public sealed class ListConsultingPackagesService
{
    private readonly IConsultingRepository _consulting;

    public ListConsultingPackagesService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<IReadOnlyList<ConsultingPackageDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _consulting.ListPublishedPackagesAsync(cancellationToken);
        return packages.Select(p => new ConsultingPackageDto(
            p.Id, p.Slug, p.Name, p.Description, p.DurationMinutes, p.Price, p.Currency)).ToList();
    }
}

public sealed class ListAvailabilitySlotsService
{
    private readonly IConsultingRepository _consulting;

    public ListAvailabilitySlotsService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<IReadOnlyList<AvailabilitySlotDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var slots = await _consulting.ListOpenSlotsAsync(DateTime.UtcNow, cancellationToken);
        return slots.Select(s => new AvailabilitySlotDto(s.Id, s.StartsAtUtc, s.EndsAtUtc)).ToList();
    }
}

public sealed class CreateBookingService
{
    private readonly IConsultingRepository _consulting;

    public CreateBookingService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<BookingDto> ExecuteAsync(Guid userId, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        var package = await _consulting.GetPackageByIdAsync(request.PackageId, cancellationToken);
        if (package is null || !package.IsPublished)
            throw new InvalidOperationException("Consulting package not found.");

        var slot = await _consulting.GetSlotByIdAsync(request.SlotId, cancellationToken);
        if (slot is null || slot.IsBlocked || slot.StartsAtUtc <= DateTime.UtcNow)
            throw new InvalidOperationException("Slot is not available.");

        if (await _consulting.IsSlotBookedAsync(request.SlotId, cancellationToken))
            throw new InvalidOperationException("Slot is already booked.");

        var booking = Booking.Create(userId, request.PackageId, request.SlotId, request.Notes);
        await _consulting.AddBookingAsync(booking, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);

        return new BookingDto(
            booking.Id,
            package.Name,
            slot.StartsAtUtc,
            slot.EndsAtUtc,
            booking.Status.ToString(),
            booking.MeetingUrl);
    }
}

public sealed class ListBookingsService
{
    private readonly IConsultingRepository _consulting;

    public ListBookingsService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<IReadOnlyList<BookingDto>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var bookings = await _consulting.ListBookingsForUserAsync(userId, cancellationToken);
        return bookings.Select(b => new BookingDto(
            b.Id,
            b.ConsultingPackage.Name,
            b.AvailabilitySlot.StartsAtUtc,
            b.AvailabilitySlot.EndsAtUtc,
            b.Status.ToString(),
            b.MeetingUrl)).ToList();
    }
}
