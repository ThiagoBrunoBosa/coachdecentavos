using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ConsultingPackageId { get; private set; }
    public Guid AvailabilitySlotId { get; private set; }
    public BookingStatus Status { get; private set; }
    public string? MeetingUrl { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ConfirmedAtUtc { get; private set; }

    public User User { get; private set; } = null!;
    public ConsultingPackage ConsultingPackage { get; private set; } = null!;
    public AvailabilitySlot AvailabilitySlot { get; private set; } = null!;

    private Booking() { }

    public static Booking Create(
        Guid userId,
        Guid consultingPackageId,
        Guid availabilitySlotId,
        string? notes = null)
    {
        return new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ConsultingPackageId = consultingPackageId,
            AvailabilitySlotId = availabilitySlotId,
            Status = BookingStatus.Pending,
            Notes = notes?.Trim(),
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void Confirm(string? meetingUrl = null)
    {
        Status = BookingStatus.Confirmed;
        ConfirmedAtUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(meetingUrl))
            MeetingUrl = meetingUrl.Trim();
    }

    public void Cancel() => Status = BookingStatus.Cancelled;

    public void Complete() => Status = BookingStatus.Completed;
}
