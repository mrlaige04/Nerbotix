using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace RoboTasker.Infrastructure;

public static class RegisterDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppAuthentication(configuration);
        
        return services;
    }

    private static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization();
        
        services.AddAuthentication().AddJwtBearer(opt =>
        {
            var bytes = Encoding.UTF8.GetBytes(configuration["Auth:JwtSecret"]!);
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(bytes),
                ValidAudience = configuration["Auth:ValidAudience"],
                ValidIssuer = configuration["Auth:ValidIssuer"],
            };
        });
    }
}