using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Consulting.Contracts;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Application.Consulting;

public sealed class ListAdminBookingsService
{
    private readonly IConsultingRepository _consulting;

    public ListAdminBookingsService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<IReadOnlyList<AdminBookingDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var bookings = await _consulting.ListAllBookingsAsync(cancellationToken);
        return bookings.Select(b => new AdminBookingDto(
            b.Id,
            b.User.Name,
            b.User.Email,
            b.ConsultingPackage.Name,
            b.AvailabilitySlot.StartsAtUtc,
            b.AvailabilitySlot.EndsAtUtc,
            b.Status.ToString())).ToList();
    }
}

public sealed class ConfirmBookingService
{
    private readonly IConsultingRepository _consulting;

    public ConfirmBookingService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task ExecuteAsync(Guid bookingId, ConfirmBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await _consulting.GetBookingByIdAsync(bookingId, cancellationToken)
            ?? throw new InvalidOperationException("Booking not found.");

        if (booking.Status != BookingStatus.Pending)
            throw new InvalidOperationException("Only pending bookings can be confirmed.");

        booking.Confirm(request.MeetingUrl);
        await _consulting.UpdateBookingAsync(booking, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CancelBookingService
{
    private readonly IConsultingRepository _consulting;

    public CancelBookingService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task ExecuteAsync(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _consulting.GetBookingForUserAsync(userId, bookingId, cancellationToken)
            ?? throw new InvalidOperationException("Booking not found.");

        if (booking.Status is BookingStatus.Cancelled or BookingStatus.Completed)
            throw new InvalidOperationException("This booking cannot be cancelled.");

        booking.Cancel();
        await _consulting.UpdateBookingAsync(booking, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);
    }
}

public sealed class AdminCancelBookingService
{
    private readonly IConsultingRepository _consulting;

    public AdminCancelBookingService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task ExecuteAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _consulting.GetBookingByIdAsync(bookingId, cancellationToken)
            ?? throw new InvalidOperationException("Booking not found.");

        if (booking.Status is BookingStatus.Cancelled or BookingStatus.Completed)
            throw new InvalidOperationException("This booking cannot be cancelled.");

        booking.Cancel();
        await _consulting.UpdateBookingAsync(booking, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);
    }
}

public sealed class AdminCompleteBookingService
{
    private readonly IConsultingRepository _consulting;

    public AdminCompleteBookingService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task ExecuteAsync(Guid bookingId, CancellationToken cancellationToken = default)
    {
        var booking = await _consulting.GetBookingByIdAsync(bookingId, cancellationToken)
            ?? throw new InvalidOperationException("Booking not found.");

        if (booking.Status != BookingStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed bookings can be completed.");

        booking.Complete();
        await _consulting.UpdateBookingAsync(booking, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ListAdminAvailabilitySlotsService
{
    private readonly IConsultingRepository _consulting;

    public ListAdminAvailabilitySlotsService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<IReadOnlyList<AdminAvailabilitySlotDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var slots = await _consulting.ListAllFutureSlotsAsync(DateTime.UtcNow, cancellationToken);
        var result = new List<AdminAvailabilitySlotDto>();

        foreach (var slot in slots)
        {
            var isBooked = await _consulting.IsSlotBookedAsync(slot.Id, cancellationToken);
            result.Add(new AdminAvailabilitySlotDto(
                slot.Id, slot.StartsAtUtc, slot.EndsAtUtc, slot.IsBlocked, isBooked));
        }

        return result;
    }
}

public sealed class CreateAdminAvailabilitySlotService
{
    private readonly IConsultingRepository _consulting;

    public CreateAdminAvailabilitySlotService(IConsultingRepository consulting) => _consulting = consulting;

    public async Task<AdminAvailabilitySlotDto> ExecuteAsync(
        CreateAvailabilitySlotRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.EndsAtUtc <= request.StartsAtUtc)
            throw new InvalidOperationException("End time must be after start time.");
        if (request.StartsAtUtc <= DateTime.UtcNow)
            throw new InvalidOperationException("Slot must be in the future.");

        var slot = AvailabilitySlot.Create(request.StartsAtUtc, request.EndsAtUtc);
        await _consulting.AddSlotAsync(slot, cancellationToken);
        await _consulting.SaveChangesAsync(cancellationToken);

        return new AdminAvailabilitySlotDto(slot.Id, slot.StartsAtUtc, slot.EndsAtUtc, slot.IsBlocked, false);
    }
}
