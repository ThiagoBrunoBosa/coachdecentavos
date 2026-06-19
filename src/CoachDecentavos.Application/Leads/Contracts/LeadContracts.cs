using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Application.Leads.Contracts;

public sealed record CreateLeadRequest(string Email, string? Name, string? Phone, string? Source, string? Message);

public sealed record LeadDto(
    Guid Id,
    string Email,
    string? Name,
    string? Phone,
    string? Source,
    string? Message,
    LeadStatus Status,
    DateTime CreatedAtUtc);