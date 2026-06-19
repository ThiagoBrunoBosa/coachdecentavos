using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Application.Auth.Contracts;

public sealed record RegisterRequest(string Name, string Email, string Password, PreferredLocale? PreferredLocale);
public sealed record LoginRequest(string Email, string Password);
public sealed record SsoLoginRequest(string IdToken, PreferredLocale? PreferredLocale);
public sealed record RefreshTokenRequest(string RefreshToken);
public sealed record LogoutRequest(string RefreshToken);

public sealed record AuthUserDto(Guid Id, string Name, string Email, UserRole Role, PreferredLocale PreferredLocale);

public sealed record AuthResponse(AuthUserDto User, string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAtUtc);