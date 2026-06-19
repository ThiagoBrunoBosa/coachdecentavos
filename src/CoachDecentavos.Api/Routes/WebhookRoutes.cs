using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Entitlements;
using CoachDecentavos.Application.Entitlements.Contracts;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Api.Routes;

public static class WebhookRoutes
{
    public static RouteGroupBuilder MapWebhookRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/webhooks/hotmart", async (
            HttpRequest httpRequest,
            HotmartWebhookRequest request,
            ProcessHotmartWebhookService service,
            IOptions<HotmartOptions> hotmartOptions,
            CancellationToken cancellationToken) =>
        {
            var hottok = httpRequest.Headers["X-Hotmart-Hottok"].FirstOrDefault();
            var expected = hotmartOptions.Value.HotTok;
            if (!string.IsNullOrWhiteSpace(expected) && hottok != expected)
                return Results.Unauthorized();

            var processed = await service.ExecuteAsync(request, cancellationToken);
            return processed ? Results.Ok(new { received = true }) : Results.BadRequest(new { received = false });
        }).AllowAnonymous();

        return group;
    }
}
