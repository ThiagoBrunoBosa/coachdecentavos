using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Infrastructure.Ai;
using CoachDecentavos.Infrastructure.Auth;
using CoachDecentavos.Infrastructure.Persistence;
using CoachDecentavos.Infrastructure.Persistence.Repositories;
using CoachDecentavos.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoachDecentavos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IEntitlementRepository, EntitlementRepository>();
        services.AddScoped<IConsultingRepository, ConsultingRepository>();
        services.AddScoped<IAiRepository, AiRepository>();
        services.AddScoped<IYouTubeShortRepository, YouTubeShortRepository>();
        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
        services.AddSingleton<IRateLimitService, InMemoryRateLimitService>();
        services.AddHttpClient<IGroqChatClient, GroqChatClient>();

        return services;
    }
}