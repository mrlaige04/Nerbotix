using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Nerbotix.Infrastructure.Hangfire;

namespace Nerbotix.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void UseBackgroundJobsDashboard(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        
        var options = configuration.GetSection(HangfireDashboardOptions.SectionName).Get<HangfireDashboardOptions>()!;
        
        app.UseHangfireDashboard(options.Url, new DashboardOptions
        {
            Authorization = [
                new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = false,
                    Users = [
                        new BasicAuthAuthorizationUser
                        {
                            Login = options.Login,
                            PasswordClear = options.Password
                        }
                    ]
                })
            ]
        });
    }    
}