using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace RoboTasker.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Auth";
    public string ValidIssuer { get; set; } = null!;
    public string ValidAudience { get; set; } = null!;
    public string JwtSecret { get; set; } = null!;
    public string TokenType { get; set; } = null!;

    public long ExpiresInMinutes { get; set; } = 60;

    public TokenValidationParameters ToTokenValidationParameters()
    {
        var bytes = Encoding.UTF8.GetBytes(JwtSecret);
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(bytes),
            ValidAudience = ValidAudience,
            ValidIssuer = ValidIssuer,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}