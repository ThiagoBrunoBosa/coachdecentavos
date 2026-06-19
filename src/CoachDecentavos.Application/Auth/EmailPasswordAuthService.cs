using BCrypt.Net;
using CoachDecentavos.Application.Auth.Contracts;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Exceptions;
using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Application.Auth;

public sealed class EmailPasswordAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly JwtOptions _jwtOptions;

    public EmailPasswordAuthService(
        IUserRepository userRepository,
        RefreshTokenService refreshTokenService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _refreshTokenService = refreshTokenService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var existing = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (existing is not null)
            throw new AppException("Email is already registered.");

        var locale = request.PreferredLocale ?? PreferredLocale.PtBr;
        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.CreateManual(request.Name, email, hash, UserRole.User, locale);
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return await BuildAuthResponseAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user is null || user.IsBlocked || string.IsNullOrEmpty(user.PasswordHash))
            throw new UnauthorizedAppException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAppException("Invalid credentials.");

        user.MarkLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return await BuildAuthResponseAsync(user, cancellationToken);
    }

    private async Task<AuthResponse> BuildAuthResponseAsync(User user, CancellationToken cancellationToken)
    {
        var (accessToken, expires) = JwtTokenGenerator.CreateAccessToken(user, _jwtOptions);
        var (refreshPlain, _) = await _refreshTokenService.IssueAsync(user, cancellationToken);
        return new AuthResponse(
            new AuthUserDto(user.Id, user.Name, user.Email, user.Role, user.PreferredLocale),
            accessToken,
            refreshPlain,
            expires);
    }
}