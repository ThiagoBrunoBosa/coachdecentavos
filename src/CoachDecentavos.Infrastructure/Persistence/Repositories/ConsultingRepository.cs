using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CoachDecentavos.Infrastructure.Persistence.Repositories;

public sealed class ConsultingRepository : IConsultingRepository
{
    private readonly AppDbContext _db;

    public ConsultingRepository(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ConsultingPackage>> ListPublishedPackagesAsync(CancellationToken cancellationToken = default)
        => await _db.ConsultingPackages
            .Where(x => x.IsPublished)
            .OrderBy(x => x.Price)
            .ToListAsync(cancellationToken);

    public Task<ConsultingPackage?> GetPackageByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.ConsultingPackages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<AvailabilitySlot>> ListOpenSlotsAsync(DateTime fromUtc, CancellationToken cancellationToken = default)
    {
        var bookedSlotIds = await _db.Bookings
            .Where(x => x.Status != BookingStatus.Cancelled)
            .Select(x => x.AvailabilitySlotId)
            .ToListAsync(cancellationToken);

        return await _db.AvailabilitySlots
            .Where(x => !x.IsBlocked && x.StartsAtUtc >= fromUtc && !bookedSlotIds.Contains(x.Id))
            .OrderBy(x => x.StartsAtUtc)
            .Take(50)
            .ToListAsync(cancellationToken);
    }

    public Task<AvailabilitySlot?> GetSlotByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.AvailabilitySlots.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> IsSlotBookedAsync(Guid slotId, CancellationToken cancellationToken = default)
        => _db.Bookings.AnyAsync(
            x => x.AvailabilitySlotId == slotId && x.Status != BookingStatus.Cancelled,
            cancellationToken);

    public async Task AddBookingAsync(Booking booking, CancellationToken cancellationToken = default)
        => await _db.Bookings.AddAsync(booking, cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListBookingsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _db.Bookings
            .Include(x => x.ConsultingPackage)
            .Include(x => x.AvailabilitySlot)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Booking>> ListAllBookingsAsync(CancellationToken cancellationToken = default)
        => await _db.Bookings
            .Include(x => x.ConsultingPackage)
            .Include(x => x.AvailabilitySlot)
            .Include(x => x.User)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public Task<Booking?> GetBookingByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.Bookings
            .Include(x => x.ConsultingPackage)
            .Include(x => x.AvailabilitySlot)
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Booking?> GetBookingForUserAsync(Guid userId, Guid bookingId, CancellationToken cancellationToken = default)
        => _db.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId && x.UserId == userId, cancellationToken);

    public Task UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        _db.Bookings.Update(booking);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _db.SaveChangesAsync(cancellationToken);

    public async Task<IReadOnlyList<AvailabilitySlot>> ListAllFutureSlotsAsync(
        DateTime fromUtc,
        CancellationToken cancellationToken = default)
        => await _db.AvailabilitySlots
            .Where(x => x.StartsAtUtc >= fromUtc)
            .OrderBy(x => x.StartsAtUtc)
            .Take(100)
            .ToListAsync(cancellationToken);

    public async Task AddSlotAsync(AvailabilitySlot slot, CancellationToken cancellationToken = default)
        => await _db.AvailabilitySlots.AddAsync(slot, cancellationToken);
}
