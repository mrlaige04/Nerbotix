using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nerbotix.Application.Common.Data;
using Nerbotix.Domain.Consts;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Data.Prefill;

public class AppDbContextSeeder(
    ILogger<AppDbContextSeeder> logger,
    ITenantRepository<Permission> permissionRepository,
    ITenantRepository<PermissionGroup> permissionGroupRepository,
    RoleManager<Role> roleManager) : ITenantSeeder
{
    public async Task SeedRolesAndPermissionsAsync(Guid tenantId)
    {
        await using var transaction = await permissionGroupRepository.BeginTransactionAsync();
        
        try
        {
            logger.LogInformation("Seeding database...");
            
            var permissionsToCreate = PermissionNames.GetAll();
            foreach (var group in permissionsToCreate)
            {
                var existingGroup = await permissionGroupRepository.GetAsync(
                    g => g.Name == group.Key && g.TenantId == tenantId);

                if (existingGroup == null)
                {
                    var newGroup = new PermissionGroup
                    {
                        Name = group.Key,
                        TenantId = tenantId,
                        IsSystem = true
                    };
                    
                    existingGroup = await permissionGroupRepository.AddAsync(newGroup);
                }
                
                foreach (var permission in group.Value)
                {
                    var existingPermission = await permissionRepository.GetAsync(
                        p => p.Name == permission && p.TenantId == tenantId && p.GroupId == existingGroup.Id);

                    if (existingPermission is { IsSystem: false })
                    {
                        existingPermission.IsSystem = true;
                        await permissionRepository.UpdateAsync(existingPermission);
                    
                        logger.LogInformation("Permission {Name} has been updated", existingPermission.Name);
                    }
                    else
                    {
                        var newPermission = new Permission
                        {
                            Name = permission,
                            IsSystem = true,
                            TenantId = tenantId,
                            GroupId = existingGroup.Id
                        };
                    
                        await permissionRepository.AddAsync(newPermission);
                    
                        logger.LogInformation("Permission {Name} has been updated", permission);
                    }
                }
            }
            
            var allPermissions = await permissionRepository.GetAllAsync(
                p => p.TenantId == tenantId,
                q => q.Include(p => p.Group));
            var adminRole = new Role
            {
                TenantId = tenantId,
                Name = RoleNames.Admin,
                IsSystem = true,
                Permissions = allPermissions.Select(p => new RolePermission
                {
                    Permission = p
                }).ToList()
            };
            await roleManager.CreateAsync(adminRole);

            var chatPermissions = allPermissions
                .Where(p => p.Group.Name == PermissionNames.ChatGroup);
            var userRole = new Role
            {
                TenantId = tenantId,
                Name = RoleNames.User,
                IsSystem = true,
                Permissions = chatPermissions.Select(p => new RolePermission
                {
                    Permission = p
                }).ToList()
            };
            await roleManager.CreateAsync(userRole);
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            logger.LogError("Error while seeding: {Message}", e.Message);
            await transaction.RollbackAsync();
        }
    }
}