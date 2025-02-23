using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Repositories.Abstractions;
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

    public static async Task EnsureSuperAdminCreated(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Tenant>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        
        var superAdminEmail = configuration["SuperAdmin:Email"];
        var superAdminPassword = configuration["SuperAdmin:Password"];
        var superTenantName = configuration["SuperAdmin:TenantName"];
        
        ArgumentNullException.ThrowIfNull(superAdminEmail, "SuperAdmin email");
        ArgumentNullException.ThrowIfNull(superAdminPassword, "SuperAdmin password");
        ArgumentNullException.ThrowIfNull(superTenantName, "SuperAdmin tenant name");
        
        var existingTenant = await tenantRepository.GetAsync(t => t.Email == superAdminEmail);
        if (existingTenant == null)
        {
            var superTenant = new Tenant
            {
                Email = superAdminEmail,
                Name = superTenantName,
            };
            
            existingTenant = await tenantRepository.AddAsync(superTenant);
        }
        
        var superUser = await userManager.FindByEmailAsync(superAdminEmail);
        var superAdmin = User.Create(superAdminEmail);
        superAdmin.Tenant = existingTenant;
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