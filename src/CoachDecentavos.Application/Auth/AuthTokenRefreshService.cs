using CoachDecentavos.Application.Auth.Contracts;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Exceptions;
using CoachDecentavos.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Application.Auth;

public sealed class AuthTokenRefreshService
{
    private readonly RefreshTokenService _refreshTokenService;
    private readonly IUserRepository _userRepository;
    private readonly JwtOptions _jwtOptions;

    public AuthTokenRefreshService(
        RefreshTokenService refreshTokenService,
        IUserRepository userRepository,
        IOptions<JwtOptions> jwtOptions)
    {
        _refreshTokenService = refreshTokenService;
        _userRepository = userRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var (_, userFromToken) = await _refreshTokenService.ValidateAsync(request.RefreshToken, cancellationToken);
        var user = await _userRepository.GetByIdAsync(userFromToken.Id, cancellationToken)
            ?? throw new UnauthorizedAppException("Invalid refresh token.");

        if (user.IsBlocked)
            throw new ForbiddenAppException("User account is blocked.");

        await _refreshTokenService.RevokeAsync(request.RefreshToken, cancellationToken);

        var (accessToken, expires) = JwtTokenGenerator.CreateAccessToken(user, _jwtOptions);
        var (refreshPlain, _) = await _refreshTokenService.IssueAsync(user, cancellationToken);

        return new AuthResponse(
            new AuthUserDto(user.Id, user.Name, user.Email, user.Role, user.PreferredLocale),
            accessToken,
            refreshPlain,
            expires);
    }
}