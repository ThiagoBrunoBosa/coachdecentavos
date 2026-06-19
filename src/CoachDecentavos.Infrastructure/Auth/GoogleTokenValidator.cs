using CoachDecentavos.Application.Common.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace CoachDecentavos.Infrastructure.Auth;

public sealed class GoogleTokenValidator : IGoogleTokenValidator
{
    private readonly string? _clientId;

    public GoogleTokenValidator(IConfiguration configuration)
    {
        _clientId = configuration["Google:ClientId"];
    }

    public async Task<GoogleUserInfo> ValidateIdTokenAsync(string idToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            throw new ArgumentException("Id token is required.", nameof(idToken));

        var settings = new GoogleJsonWebSignature.ValidationSettings();
        if (!string.IsNullOrWhiteSpace(_clientId))
            settings.Audience = new[] { _clientId };

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        cancellationToken.ThrowIfCancellationRequested();

        return new GoogleUserInfo(
            payload.Subject,
            payload.Email,
            payload.Name ?? payload.Email,
            payload.EmailVerified);
    }
}