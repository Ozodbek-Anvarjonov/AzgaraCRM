using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Domain.Settings;
using AzgaraCRM.WebUI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AzgaraCRM.WebUI.Services;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JwtSettings _jwtSettings;

    public TokenGeneratorService(IOptions<JwtSettings> jwtSettings) =>
        _jwtSettings = jwtSettings.Value;

    public Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var jwtSecurityToken = JwtSecurityToken(user);
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Task.FromResult(token);
    }

    private JwtSecurityToken JwtSecurityToken(User user)
    {
        var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
        var claims = GetClaims(user);

        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(_jwtSettings.ExpirationTimeInSeconds),
            signingCredentials: credentials
            );
    }

    private IList<Claim> GetClaims(User user)
    {
        return new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
    }
}