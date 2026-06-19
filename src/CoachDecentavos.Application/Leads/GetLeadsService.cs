using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Leads.Contracts;

namespace CoachDecentavos.Application.Leads;

public sealed class GetLeadsService
{
    private readonly ILeadRepository _leadRepository;

    public GetLeadsService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }

    public async Task<IReadOnlyList<LeadDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var leads = await _leadRepository.ListAsync(cancellationToken);
        return leads
            .Select(l => new LeadDto(l.Id, l.Email, l.Name, l.Phone, l.Source, l.Message, l.Status, l.CreatedAtUtc))
            .ToList();
    }
}