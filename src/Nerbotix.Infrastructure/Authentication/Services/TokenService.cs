using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Nerbotix.Infrastructure.Authentication.Services;

public class TokenService(
    IOptions<JwtOptions> jwtOptions, ITenantRepository<User> userRepository,
    IConfiguration configuration)
{
    public async Task<AccessTokenResponse> GenerateToken(User user)
    {
        var options = jwtOptions.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var superAdminEmail = configuration["SuperAdmin:Email"];
        var isSuperAdmin = user.Email!.Equals(superAdminEmail, StringComparison.InvariantCultureIgnoreCase);
        
        var userRoles = await userRepository.GetWithSelectorAsync(
            u => u.Roles.Select(ur => ur.Role),
            u => u.Id == user.Id,
            q => q
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role));

        var roleClaims = userRoles?
            .Select(ur => new Claim("role", ur.Name!))
            .ToList();
        
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(CustomClaims.TenantId, user.TenantId.ToString()),
                ..roleClaims ?? [],
                new Claim("IsSuperAdmin", isSuperAdmin.ToString().ToLowerInvariant()),
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

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }

    public string? GetEmailFromExpiredToken(string token)
    {
        var tokenHandler = new JsonWebTokenHandler();

        try
        {
            var readToken = tokenHandler.ReadJsonWebToken(token);
            return readToken.TryGetValue(JwtRegisteredClaimNames.Email, out string? email) ? email : null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}