using CoachDecentavos.Application.Leads;
using CoachDecentavos.Application.Leads.Contracts;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Api.Routes;

public static class LeadRoutes
{
    public static RouteGroupBuilder MapLeadRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/leads", async (
            CreateLeadRequest request,
            CreateLeadService createLeadService,
            CancellationToken cancellationToken) =>
        {
            var lead = await createLeadService.ExecuteAsync(request, cancellationToken);
            return Results.Created($"/api/v1/admin/leads/{lead.Id}", lead);
        }).AllowAnonymous();

        group.MapGet("/admin/leads", async (
            GetLeadsService getLeadsService,
            CancellationToken cancellationToken) =>
        {
            var leads = await getLeadsService.ExecuteAsync(cancellationToken);
            return Results.Ok(leads);
        })
        .RequireAuthorization(policy => policy.RequireRole(nameof(UserRole.Admin)));

        return group;
    }
}