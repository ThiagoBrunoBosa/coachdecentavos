using CoachDecentavos.Application.Auth.Contracts;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Exceptions;
using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Application.Auth;

public sealed class UpsertUserFromSsoService
{
    private const string GoogleProvider = "google";

    private readonly IUserRepository _userRepository;
    private readonly IGoogleTokenValidator _googleTokenValidator;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly JwtOptions _jwtOptions;

    public UpsertUserFromSsoService(
        IUserRepository userRepository,
        IGoogleTokenValidator googleTokenValidator,
        RefreshTokenService refreshTokenService,
        IOptions<JwtOptions> jwtOptions)
    {
        _userRepository = userRepository;
        _googleTokenValidator = googleTokenValidator;
        _refreshTokenService = refreshTokenService;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponse> LoginWithGoogleAsync(SsoLoginRequest request, CancellationToken cancellationToken = default)
    {
        var googleUser = await _googleTokenValidator.ValidateIdTokenAsync(request.IdToken, cancellationToken);
        if (!googleUser.EmailVerified)
            throw new UnauthorizedAppException("Google email is not verified.");

        var locale = request.PreferredLocale ?? PreferredLocale.PtBr;
        var user = await _userRepository.GetByOAuthAsync(GoogleProvider, googleUser.Subject, cancellationToken);

        if (user is null)
        {
            user = await _userRepository.GetByEmailAsync(googleUser.Email, cancellationToken);
            if (user is null)
            {
                user = User.CreateFromSso(googleUser.Name, googleUser.Email, locale);
                user.OAuthAccounts.Add(UserOAuthAccount.Create(user.Id, GoogleProvider, googleUser.Subject));
                await _userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                user.OAuthAccounts.Add(UserOAuthAccount.Create(user.Id, GoogleProvider, googleUser.Subject));
                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }

        if (user.IsBlocked)
            throw new ForbiddenAppException("User account is blocked.");

        user.MarkLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var (accessToken, expires) = JwtTokenGenerator.CreateAccessToken(user, _jwtOptions);
        var (refreshPlain, _) = await _refreshTokenService.IssueAsync(user, cancellationToken);

        return new AuthResponse(
            new AuthUserDto(user.Id, user.Name, user.Email, user.Role, user.PreferredLocale),
            accessToken,
            refreshPlain,
            expires);
    }
}