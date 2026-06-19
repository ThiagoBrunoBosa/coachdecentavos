using System.Security.Cryptography;
using System.Text;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Exceptions;
using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Auth;
using CoachDecentavos.Domain.Entities;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Application.Auth;

public sealed class RefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IOptions<JwtOptions> jwtOptions)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<(string PlainToken, RefreshToken Entity)> IssueAsync(User user, CancellationToken cancellationToken = default)
    {
        var plain = GeneratePlainToken();
        var hash = HashToken(plain);
        var entity = RefreshToken.Create(user.Id, hash, DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays));
        await _refreshTokenRepository.AddAsync(entity, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
        return (plain, entity);
    }

    public async Task<(RefreshToken Entity, User User)> ValidateAsync(string plainToken, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(plainToken);
        var entity = await _refreshTokenRepository.GetByTokenHashAsync(hash, cancellationToken);
        if (entity is null || !entity.IsActive)
            throw new UnauthorizedAppException("Invalid refresh token.");

        return (entity, entity.User);
    }

    public async Task RevokeAsync(string plainToken, CancellationToken cancellationToken = default)
    {
        var hash = HashToken(plainToken);
        var entity = await _refreshTokenRepository.GetByTokenHashAsync(hash, cancellationToken);
        if (entity is null)
            return;

        entity.Revoke();
        await _refreshTokenRepository.UpdateAsync(entity, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);
    }

    public static string HashToken(string plainToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainToken));
        return Convert.ToHexString(bytes);
    }

    private static string GeneratePlainToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}