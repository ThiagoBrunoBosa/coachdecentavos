using CoachDecentavos.Application.Common;

namespace CoachDecentavos.Api.Routes;

public static class HealthRoutes
{
    public static RouteGroupBuilder MapHealthRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/health", () => Results.Ok(new HealthResponse("Healthy", DateTime.UtcNow)))
            .AllowAnonymous()
            .WithName("HealthCheck");

        return group;
    }
}