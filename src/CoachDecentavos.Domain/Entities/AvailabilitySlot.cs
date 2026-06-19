namespace CoachDecentavos.Domain.Entities;

public class AvailabilitySlot
{
    public Guid Id { get; private set; }
    public DateTime StartsAtUtc { get; private set; }
    public DateTime EndsAtUtc { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private AvailabilitySlot() { }

    public static AvailabilitySlot Create(DateTime startsAtUtc, DateTime endsAtUtc)
    {
        if (endsAtUtc <= startsAtUtc)
            throw new ArgumentException("End must be after start.");

        return new AvailabilitySlot
        {
            Id = Guid.NewGuid(),
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            IsBlocked = false,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void Block() => IsBlocked = true;
}
