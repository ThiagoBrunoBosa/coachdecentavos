using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace CoachDecentavos.Application.Auth;

public static class JwtTokenGenerator
{
    public static (string Token, DateTime ExpiresAtUtc) CreateAccessToken(User user, JwtOptions options)
    {
        var expires = DateTime.UtcNow.AddMinutes(options.AccessTokenMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("locale", user.PreferredLocale.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var handler = new JwtSecurityTokenHandler();
        return (handler.WriteToken(token), expires);
    }
}