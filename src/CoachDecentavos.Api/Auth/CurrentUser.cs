using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CoachDecentavos.Api.Auth;

public static class CurrentUser
{
    public static Guid? GetUserId(ClaimsPrincipal principal)
    {
        var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}
