using CoachDecentavos.Application.Ai;
using CoachDecentavos.Application.Auth;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Consulting;
using CoachDecentavos.Application.Entitlements;
using CoachDecentavos.Application.Leads;
using CoachDecentavos.Application.Products;
using CoachDecentavos.Application.Shorts;
using Microsoft.Extensions.DependencyInjection;

namespace CoachDecentavos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.SectionName);
        services.AddOptions<LlmOptions>().BindConfiguration(LlmOptions.SectionName);
        services.AddOptions<HotmartOptions>().BindConfiguration(HotmartOptions.SectionName);
        services.AddOptions<InternalOptions>().BindConfiguration(InternalOptions.SectionName);

        services.AddScoped<EmailPasswordAuthService>();
        services.AddScoped<UpsertUserFromSsoService>();
        services.AddScoped<RefreshTokenService>();
        services.AddScoped<AuthTokenRefreshService>();
        services.AddScoped<CreateLeadService>();
        services.AddScoped<GetLeadsService>();
        services.AddScoped<ListProductsService>();
        services.AddScoped<GetProductService>();
        services.AddScoped<ProcessHotmartWebhookService>();
        services.AddScoped<ListEntitlementsService>();
        services.AddScoped<LinkEntitlementService>();
        services.AddScoped<ListConsultingPackagesService>();
        services.AddScoped<ListAvailabilitySlotsService>();
        services.AddScoped<CreateBookingService>();
        services.AddScoped<ListBookingsService>();
        services.AddScoped<ListAdminBookingsService>();
        services.AddScoped<ConfirmBookingService>();
        services.AddScoped<CancelBookingService>();
        services.AddScoped<AdminCancelBookingService>();
        services.AddScoped<AdminCompleteBookingService>();
        services.AddScoped<ListAdminAvailabilitySlotsService>();
        services.AddScoped<CreateAdminAvailabilitySlotService>();
        services.AddScoped<FinancialAssistantService>();
        services.AddScoped<ListYouTubeShortsService>();

        return services;
    }
}