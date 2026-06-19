using CoachDecentavos.Application.Auth;
using CoachDecentavos.Application.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CoachDecentavos.Api.Routes;

public static class AuthRoutes
{
    public static RouteGroupBuilder MapAuthRoutes(this RouteGroupBuilder group)
    {
        var auth = group.MapGroup("/auth");

        auth.MapPost("/register", async (
            RegisterRequest request,
            EmailPasswordAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return Results.Ok(response);
        }).AllowAnonymous();

        auth.MapPost("/login", async (
            LoginRequest request,
            EmailPasswordAuthService authService,
            CancellationToken cancellationToken) =>
        {
            var response = await authService.LoginAsync(request, cancellationToken);
            return Results.Ok(response);
        }).AllowAnonymous();

        auth.MapPost("/sso", async (
            SsoLoginRequest request,
            UpsertUserFromSsoService ssoService,
            CancellationToken cancellationToken) =>
        {
            var response = await ssoService.LoginWithGoogleAsync(request, cancellationToken);
            return Results.Ok(response);
        }).AllowAnonymous();

        auth.MapPost("/refresh", async (
            RefreshTokenRequest request,
            AuthTokenRefreshService refreshService,
            CancellationToken cancellationToken) =>
        {
            var response = await refreshService.RefreshAsync(request, cancellationToken);
            return Results.Ok(response);
        }).AllowAnonymous();

        auth.MapPost("/logout", async (
            LogoutRequest request,
            RefreshTokenService refreshTokenService,
            CancellationToken cancellationToken) =>
        {
            await refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        return group;
    }
}