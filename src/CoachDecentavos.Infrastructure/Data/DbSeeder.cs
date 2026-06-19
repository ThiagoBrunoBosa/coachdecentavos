using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using CoachDecentavos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoachDecentavos.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IHostEnvironment environment,
        CancellationToken cancellationToken = default)
    {
        await SeedAdminUserAsync(services, cancellationToken);
        await SeedConfiguredUserAsync(services, cancellationToken);

        if (environment.IsDevelopment() || environment.IsEnvironment("Docker"))
            await SeedDemoContentAsync(services, cancellationToken);
    }

    public static async Task SeedAdminUserAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        var email = Environment.GetEnvironmentVariable("ADMIN_SEED_EMAIL");
        var password = Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD");
        var name = Environment.GetEnvironmentVariable("ADMIN_SEED_NAME") ?? "Admin";

        await SeedManualUserIfMissingAsync(
            services,
            name,
            email,
            password,
            UserRole.Admin,
            PreferredLocale.PtBr,
            cancellationToken);
    }

    public static async Task SeedConfiguredUserAsync(
        IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        var name = Environment.GetEnvironmentVariable("USER_SEED_NAME");
        var email = Environment.GetEnvironmentVariable("USER_SEED_EMAIL");
        var password = Environment.GetEnvironmentVariable("USER_SEED_PASSWORD");

        await SeedManualUserIfMissingAsync(
            services,
            name,
            email,
            password,
            UserRole.User,
            PreferredLocale.PtBr,
            cancellationToken);
    }

    private static async Task SeedManualUserIfMissingAsync(
        IServiceProvider services,
        string? name,
        string? email,
        string? password,
        UserRole role,
        PreferredLocale locale,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name)
            || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password))
            return;

        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        var normalizedEmail = email.Trim().ToLowerInvariant();
        var exists = await dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (exists)
            return;

        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = User.CreateManual(name.Trim(), normalizedEmail, hash, role, locale);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Seeded {Role} user {Email}", role, normalizedEmail);
    }

    private static async Task SeedDemoContentAsync(IServiceProvider services, CancellationToken cancellationToken)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        if (!await db.Products.AnyAsync(cancellationToken))
        {
            var budget = Product.Create("budget-basics", "Budget Basics", ProductType.Course, 97m);
            budget.ConfigureHotmart("demo-budget-basics", "https://pay.hotmart.com/demo-budget-basics");
            budget.Publish();

            var debt = Product.Create("debt-freedom", "Debt Freedom", ProductType.Course, 147m);
            debt.ConfigureHotmart("demo-debt-freedom", "https://pay.hotmart.com/demo-debt-freedom");
            debt.Publish();

            db.Products.AddRange(budget, debt);
            logger.LogInformation("Seeded demo products.");
        }

        if (!await db.ConsultingPackages.AnyAsync(cancellationToken))
        {
            var starter = ConsultingPackage.Create(
                "starter-session",
                "Starter Session",
                60,
                350m,
                "One-hour financial clarity session with Carolyne.");
            starter.Publish();

            db.ConsultingPackages.Add(starter);
            logger.LogInformation("Seeded demo consulting package.");
        }

        if (!await db.AvailabilitySlots.AnyAsync(cancellationToken))
        {
            var start = DateTime.UtcNow.Date.AddDays(3).AddHours(14);
            db.AvailabilitySlots.Add(AvailabilitySlot.Create(start, start.AddHours(1)));
            db.AvailabilitySlots.Add(AvailabilitySlot.Create(start.AddDays(1), start.AddDays(1).AddHours(1)));
            logger.LogInformation("Seeded demo availability slots.");
        }

        if (!await db.YouTubeShorts.AnyAsync(cancellationToken))
        {
            db.YouTubeShorts.Add(YouTubeShort.Create(
                "dQw4w9WgXcQ",
                "3 tips to start investing",
                0,
                "https://img.youtube.com/vi/dQw4w9WgXcQ/hqdefault.jpg"));
            db.YouTubeShorts.Add(YouTubeShort.Create(
                "jNQXAC9IVRw",
                "How to build an emergency fund",
                1,
                "https://img.youtube.com/vi/jNQXAC9IVRw/hqdefault.jpg"));
            db.YouTubeShorts.Add(YouTubeShort.Create(
                "9bZkp7q19f0",
                "Avoid these budget mistakes",
                2,
                "https://img.youtube.com/vi/9bZkp7q19f0/hqdefault.jpg"));
            logger.LogInformation("Seeded demo YouTube shorts.");
        }

        await db.SaveChangesAsync(cancellationToken);

        await SeedDemoUserAsync(db, logger, cancellationToken);
    }

    private static async Task SeedDemoUserAsync(
        AppDbContext db,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        const string demoEmail = "demo@local.dev";
        const string demoPassword = "Demo123456!";

        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == demoEmail, cancellationToken);
        if (user is null)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(demoPassword);
            user = User.CreateManual("Demo User", demoEmail, hash, UserRole.User, PreferredLocale.PtBr);
            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Seeded demo user {Email}", demoEmail);
        }

        var budgetProduct = await db.Products.FirstOrDefaultAsync(x => x.Slug == "budget-basics", cancellationToken);
        if (budgetProduct is null)
            return;

        var hasActiveEntitlement = await db.UserEntitlements.AnyAsync(
            x => x.UserId == user.Id && x.ProductId == budgetProduct.Id,
            cancellationToken);
        if (!hasActiveEntitlement)
        {
            var active = UserEntitlement.CreateFromHotmart(
                budgetProduct.Id,
                demoEmail,
                "demo-tx-active-001",
                user.Id);
            db.UserEntitlements.Add(active);
            logger.LogInformation("Seeded active entitlement for demo user.");
        }

        var debtProduct = await db.Products.FirstOrDefaultAsync(x => x.Slug == "debt-freedom", cancellationToken);
        if (debtProduct is null)
            return;

        var hasPending = await db.UserEntitlements.AnyAsync(
            x => x.BuyerEmail == demoEmail && x.ProductId == debtProduct.Id && x.UserId == null,
            cancellationToken);
        if (!hasPending)
        {
            var pending = UserEntitlement.CreateFromHotmart(
                debtProduct.Id,
                demoEmail,
                "demo-tx-pending-001");
            db.UserEntitlements.Add(pending);
            logger.LogInformation("Seeded pending entitlement for link-purchase demo.");
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
