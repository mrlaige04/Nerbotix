using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RoboTasker.Domain.Tenants;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RoboTasker.Infrastructure.Authentication.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions)
{
    public AccessTokenResponse GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(CustomClaims.TenantId, user.TenantId.ToString())
        };
        
        var options = jwtOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: options.ValidIssuer,
            audience: options.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(options.ExpiresInMinutes),
            signingCredentials: credentials);
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessTokenResponse
        {
            AccessToken = accessToken,
            TokenType = options.TokenType,
        };
    }
}