using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RChat.Application.Abstractions.Services.Authentication;
using RChat.Application.Accounts.Dtos;

namespace RChat.Infrastructure.Services.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly SymmetricSecurityKey _key = 
        new (Encoding.UTF8.GetBytes(options.Value.Key));
    
    public string GenerateAccessToken(int accountId, string roleName)
    {
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, accountId.ToString()),
            new (ClaimTypes.Role, roleName)
        };
        
        var token = new JwtSecurityToken(
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.AddMinutes(options.Value.LifeTimeInMinutes),
            audience: options.Value.Audience,
            issuer: options.Value.Issuer,
            claims: claims
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}