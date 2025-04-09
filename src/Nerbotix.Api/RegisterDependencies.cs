using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Nerbotix.Api.Infrastructure;
using Nerbotix.Api.Services;
using Nerbotix.Domain.Services;

namespace Nerbotix.Api;

public static class RegisterDependencies
{
    public static IServiceCollection AddUi(this IServiceCollection services)
    {
        services.Configure<FormOptions>(f =>
        {
            f.MultipartBodyLengthLimit = 100000000 * 2; // TODO: should move to appsettings
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                       Reference = new OpenApiReference
                       {
                           Type = ReferenceType.SecurityScheme,
                           Id = "Bearer"
                       }
                    }, []
                }
            });
        });
        
        services.AddControllers();

        services.AddCors(cors =>
        {
            cors.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        services.AddHttpContextAccessor();

        services.AddExceptionHandler<RoboExceptionHandler>();

        services.AddScoped<ICurrentUser, CurrentUser>();
        
        return services;
    }
}