using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Tenants;
using RoboTasker.Infrastructure.Data;

namespace RoboTasker.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<RoboTaskerDbContext>();

        if (await context.Database.CanConnectAsync())
        {
            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();
        }
    }

    public static async Task EnsureSeedData(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<RoboTaskerDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var superAdminEmail = configuration["SuperAdmin:Email"];
        var superAdminPassword = configuration["SuperAdmin:Password"];
        
        ArgumentNullException.ThrowIfNull(superAdminEmail, "SuperAdmin email");
        ArgumentNullException.ThrowIfNull(superAdminPassword, "SuperAdmin password");
        
        var superAdmin = User.Create(superAdminEmail);
        
        var superUser = await userManager.FindByEmailAsync(superAdminEmail);
        if (superUser == null)
        {
            var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}