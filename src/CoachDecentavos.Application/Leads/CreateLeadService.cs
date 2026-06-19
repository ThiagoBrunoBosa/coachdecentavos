using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Application.Leads.Contracts;
using CoachDecentavos.Domain.Entities;

namespace CoachDecentavos.Application.Leads;

public sealed class CreateLeadService
{
    private readonly ILeadRepository _leadRepository;

    public CreateLeadService(ILeadRepository leadRepository)
    {
        _leadRepository = leadRepository;
    }

    public async Task<LeadDto> ExecuteAsync(CreateLeadRequest request, CancellationToken cancellationToken = default)
    {
        var lead = LeadInterest.Create(request.Email, request.Name, request.Phone, request.Source, request.Message);
        await _leadRepository.AddAsync(lead, cancellationToken);
        await _leadRepository.SaveChangesAsync(cancellationToken);

        return new LeadDto(lead.Id, lead.Email, lead.Name, lead.Phone, lead.Source, lead.Message, lead.Status, lead.CreatedAtUtc);
    }
}