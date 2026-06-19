namespace CoachDecentavos.Application.Common.Interfaces;

public interface IGoogleTokenValidator
{
    Task<GoogleUserInfo> ValidateIdTokenAsync(string idToken, CancellationToken cancellationToken = default);
}

public sealed record GoogleUserInfo(string Subject, string Email, string Name, bool EmailVerified);