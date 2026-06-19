using CoachDecentavos.Application.Shorts;

namespace CoachDecentavos.Api.Routes;

public static class ShortRoutes
{
    public static RouteGroupBuilder MapShortRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/shorts", async (
            ListYouTubeShortsService service,
            string? sort,
            int? limit,
            CancellationToken cancellationToken) =>
        {
            if (string.Equals(sort, "latest", StringComparison.OrdinalIgnoreCase) && limit is > 0)
            {
                var latest = await service.ExecuteLatestAsync(limit.Value, cancellationToken);
                return Results.Ok(latest);
            }

            var items = await service.ExecuteAsync(cancellationToken);
            return Results.Ok(items);
        }).AllowAnonymous();

        return group;
    }
}
