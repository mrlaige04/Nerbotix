using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RoboTasker.Domain.Tenants;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace RoboTasker.Infrastructure.Authentication.Services;

public class TokenService(IOptions<JwtOptions> jwtOptions)
{
    public AccessTokenResponse GenerateToken(User user)
    {
        var options = jwtOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(CustomClaims.TenantId, user.TenantId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = credentials,
            Issuer = options.ValidIssuer,
            Audience = options.ValidAudience
        };

        var handler = new JsonWebTokenHandler();
        
        var token = handler.CreateToken(securityTokenDescriptor);
        return new AccessTokenResponse
        {
            AccessToken = token,
            TokenType = options.TokenType,
        };
    }
}