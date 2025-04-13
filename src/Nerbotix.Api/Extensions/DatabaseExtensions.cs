using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Domain.Consts;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;
using Nerbotix.Infrastructure.Data;

namespace Nerbotix.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<NerbotixDbContext>();

        if (await context.Database.CanConnectAsync())
        {
            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();
            await app.EnsureSuperAdminCreated();
        }
    }

    public static async Task EnsureSuperAdminCreated(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<IBaseRepository<Tenant>>();
        var roleRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository<Role>>();
        var userRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        
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
        
        var existingSuperRole = await roleRepository.GetAsync(
            t => t.Name == RoleNames.SuperAdmin && t.TenantId == existingTenant.Id);
        if (existingSuperRole == null)
        {
            var result = await roleManager.CreateAsync(new Role
            {
                Name = RoleNames.SuperAdmin,
                TenantId = existingTenant.Id
            });
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
            existingSuperRole = await roleRepository.GetAsync(t => t.Name == RoleNames.SuperAdmin && t.TenantId == existingTenant.Id);
        }

        var superAdminUser = await userRepository.GetAsync(
            t => t.Email == superAdminEmail && t.TenantId == existingTenant.Id,
            q => q.Include(u => u.Roles)
                .ThenInclude(ur => ur.Role));
        if (superAdminUser!.Roles.All(r => r.Role.Name != RoleNames.SuperAdmin))
        {
            superAdminUser!.Roles.Add(new UserRole
            {
                RoleId = existingSuperRole!.Id,
                TenantId = existingTenant.Id
            });
            
            await userRepository.UpdateAsync(superAdminUser);
        }
    }
}