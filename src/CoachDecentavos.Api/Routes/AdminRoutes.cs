using CoachDecentavos.Application.Consulting;
using CoachDecentavos.Application.Consulting.Contracts;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Api.Routes;

public static class AdminRoutes
{
    public static RouteGroupBuilder MapAdminRoutes(this RouteGroupBuilder group)
    {
        var admin = group.MapGroup("/admin")
            .RequireAuthorization(policy => policy.RequireRole(nameof(UserRole.Admin)));

        admin.MapGet("/bookings", async (
            ListAdminBookingsService service,
            CancellationToken cancellationToken) =>
        {
            var bookings = await service.ExecuteAsync(cancellationToken);
            return Results.Ok(bookings);
        });

        admin.MapPost("/bookings/{id:guid}/confirm", async (
            Guid id,
            ConfirmBookingRequest request,
            ConfirmBookingService service,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await service.ExecuteAsync(id, request, cancellationToken);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        admin.MapPost("/bookings/{id:guid}/cancel", async (
            Guid id,
            AdminCancelBookingService service,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await service.ExecuteAsync(id, cancellationToken);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        admin.MapPost("/bookings/{id:guid}/complete", async (
            Guid id,
            AdminCompleteBookingService service,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await service.ExecuteAsync(id, cancellationToken);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        admin.MapGet("/slots", async (
            ListAdminAvailabilitySlotsService service,
            CancellationToken cancellationToken) =>
        {
            var slots = await service.ExecuteAsync(cancellationToken);
            return Results.Ok(slots);
        });

        admin.MapPost("/slots", async (
            CreateAvailabilitySlotRequest request,
            CreateAdminAvailabilitySlotService service,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var slot = await service.ExecuteAsync(request, cancellationToken);
                return Results.Created($"/api/v1/admin/slots/{slot.Id}", slot);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        return group;
    }
}
