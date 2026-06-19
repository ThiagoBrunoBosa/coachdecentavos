using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class LeadInterest
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? Source { get; private set; }
    public string? Message { get; private set; }
    public LeadStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private LeadInterest() { }

    public static LeadInterest Create(string email, string? name, string? phone, string? source, string? message)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        return new LeadInterest
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim(),
            Source = string.IsNullOrWhiteSpace(source) ? null : source.Trim(),
            Message = string.IsNullOrWhiteSpace(message) ? null : message.Trim(),
            Status = LeadStatus.New,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}