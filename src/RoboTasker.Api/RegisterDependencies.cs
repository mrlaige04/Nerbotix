using RoboTasker.Api.Infrastructure;

namespace RoboTasker.Api;

public static class RegisterDependencies
{
    public static IServiceCollection AddUi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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
        
        return services;
    }
}