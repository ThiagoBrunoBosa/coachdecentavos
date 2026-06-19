using CoachDecentavos.Api.Auth;
using CoachDecentavos.Application.Ai;
using CoachDecentavos.Application.Consulting;
using CoachDecentavos.Application.Consulting.Contracts;
using CoachDecentavos.Application.Entitlements;
using CoachDecentavos.Application.Entitlements.Contracts;
using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Api.Routes;

public static class MeRoutes
{
    public static RouteGroupBuilder MapMeRoutes(this RouteGroupBuilder group)
    {
        var me = group.MapGroup("/me").RequireAuthorization();

        me.MapGet("/entitlements", async (
            HttpContext httpContext,
            ListEntitlementsService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            var items = await service.ExecuteAsync(userId.Value, cancellationToken);
            return Results.Ok(items);
        });

        me.MapPost("/entitlements/link-purchase", async (
            HttpContext httpContext,
            LinkPurchaseRequest request,
            LinkEntitlementService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            try
            {
                var linked = await service.ExecuteAsync(userId.Value, request.BuyerEmail, cancellationToken);
                return Results.Ok(new { linkedCount = linked });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        me.MapGet("/bookings", async (
            HttpContext httpContext,
            ListBookingsService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            var items = await service.ExecuteAsync(userId.Value, cancellationToken);
            return Results.Ok(items);
        });

        me.MapPost("/bookings", async (
            HttpContext httpContext,
            CreateBookingRequest request,
            CreateBookingService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            try
            {
                var booking = await service.ExecuteAsync(userId.Value, request, cancellationToken);
                return Results.Created($"/api/v1/me/bookings/{booking.Id}", booking);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        me.MapPost("/bookings/{id:guid}/cancel", async (
            Guid id,
            HttpContext httpContext,
            CancelBookingService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            try
            {
                await service.ExecuteAsync(userId.Value, id, cancellationToken);
                return Results.NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        me.MapGet("/ai/consent", async (
            HttpContext httpContext,
            FinancialAssistantService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            var hasConsent = await service.HasConsentAsync(userId.Value, cancellationToken);
            return Results.Ok(new { hasConsent, version = FinancialAssistantService.CurrentDisclaimerVersion });
        });

        me.MapPost("/ai/consent", async (
            HttpContext httpContext,
            AcceptAiConsentRequest request,
            FinancialAssistantService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();
            await service.AcceptConsentAsync(userId.Value, request.DisclaimerVersion, cancellationToken);
            return Results.NoContent();
        });

        me.MapPost("/ai/ask", async (
            HttpContext httpContext,
            AskAssistantRequest request,
            FinancialAssistantService service,
            CancellationToken cancellationToken) =>
        {
            var userId = CurrentUser.GetUserId(httpContext.User);
            if (userId is null) return Results.Unauthorized();

            var localeClaim = httpContext.User.FindFirst("locale")?.Value;
            var locale = localeClaim == nameof(PreferredLocale.EnUs)
                ? PreferredLocale.EnUs
                : PreferredLocale.PtBr;

            try
            {
                var response = await service.AskAsync(userId.Value, locale, request, cancellationToken);
                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        return group;
    }
}

public static class ConsultingRoutes
{
    public static RouteGroupBuilder MapConsultingRoutes(this RouteGroupBuilder group)
    {
        var consulting = group.MapGroup("/consulting");

        consulting.MapGet("/packages", async (
            ListConsultingPackagesService service,
            CancellationToken cancellationToken) =>
        {
            var items = await service.ExecuteAsync(cancellationToken);
            return Results.Ok(items);
        }).AllowAnonymous();

        consulting.MapGet("/slots", async (
            ListAvailabilitySlotsService service,
            CancellationToken cancellationToken) =>
        {
            var items = await service.ExecuteAsync(cancellationToken);
            return Results.Ok(items);
        }).AllowAnonymous();

        return group;
    }
}

public static class InternalRoutes
{
    public static RouteGroupBuilder MapInternalRoutes(this RouteGroupBuilder group)
    {
        group.MapPost("/internal/youtube/sync", async (
            HttpRequest request,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("YouTubeSync");
            var secret = configuration["Internal:CronSecret"];
            var header = request.Headers["X-Cron-Secret"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(secret) && header != secret)
                return Results.Unauthorized();

            var apiKey = configuration["YouTube:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.LogInformation("YouTube sync skipped: API key not configured.");
                return Results.Ok(new { synced = 0, skipped = true });
            }

            logger.LogInformation("YouTube sync endpoint called.");
            await Task.CompletedTask;
            return Results.Ok(new { synced = 0, message = "Sync stub ready for YouTube API integration." });
        }).AllowAnonymous();

        return group;
    }
}
