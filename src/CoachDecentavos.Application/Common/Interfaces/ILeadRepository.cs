using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Common.Interfaces;

public interface ILeadRepository
{
    Task AddAsync(LeadInterest lead, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LeadInterest>> ListAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}