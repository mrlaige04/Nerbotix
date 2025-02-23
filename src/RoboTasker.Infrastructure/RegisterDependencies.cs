using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoboTasker.Application.Services;
using RoboTasker.Domain.Repositories;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;
using RoboTasker.Infrastructure.Authentication;
using RoboTasker.Infrastructure.Authentication.Providers;
using RoboTasker.Infrastructure.Authentication.Services;
using RoboTasker.Infrastructure.Data;
using RoboTasker.Infrastructure.Emailing;

namespace RoboTasker.Infrastructure;

public static class RegisterDependencies
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppAuthentication(configuration);
        services.AddDatabase(configuration);

        services.AddEmailing(configuration);
        
        return services;
    }

    private static void AddEmailing(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(EmailOptions.SectionName).Get<EmailOptions>();
        ArgumentNullException.ThrowIfNull(options);
        
        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services
            .AddFluentEmail(options.From)
            .AddSmtpSender(new SmtpClient(options.Host)
            {
                Port = options.Port,
                Credentials = new NetworkCredential(options.From, options.Password),
                EnableSsl = true,
                UseDefaultCredentials = false
            });

        services.AddScoped<IEmailSender, EmailSender>();
    }
    
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RoboTaskerDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions
                    .UseNetTopologySuite()
                    .MigrationsAssembly(typeof(RoboTaskerDbContext).Assembly)
            );
        });
        
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<RoboTaskerDbContext>());
        
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped(typeof(ITenantRepository<>), typeof(TenantBaseRepository<>));
    }
    
    private static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions);
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddIdentity<User, Role>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<NumericEmailTokenProvider<User>>("NumericEmail")
            .AddEntityFrameworkStores<RoboTaskerDbContext>();

        services.AddScoped<TokenService>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Tokens.PasswordResetTokenProvider = "NumericEmail";
        });
        
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = jwtOptions.ToTokenValidationParameters();
            });
    }
}