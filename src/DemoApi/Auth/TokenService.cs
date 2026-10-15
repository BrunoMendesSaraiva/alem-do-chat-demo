using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DemoApi.Users;
using Microsoft.IdentityModel.Tokens;

namespace DemoApi.Auth;

public class TokenService
{
    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            issuer: TokenSettings.Issuer,
            audience: TokenSettings.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(TokenSettings.ExpirationMinutes),
            signingCredentials: new SigningCredentials(GetSigningKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static TokenValidationParameters CreateValidationParameters() => new()
    {
        ValidIssuer = TokenSettings.Issuer,
        ValidAudience = TokenSettings.Audience,
        IssuerSigningKey = GetSigningKey(),
        ClockSkew = TimeSpan.Zero
    };

    private static SymmetricSecurityKey GetSigningKey() =>
        new(Encoding.UTF8.GetBytes(TokenSettings.SigningKey));
}
