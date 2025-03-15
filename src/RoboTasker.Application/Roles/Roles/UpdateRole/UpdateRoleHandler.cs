using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Tenants;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Roles.UpdateRole;

public class UpdateRoleHandler(
    ITenantRepository<Role> roleRepository,
    ITenantRepository<Permission> permissionRepository) : ICommandHandler<UpdateRoleCommand, RoleBaseResponse>
{
    public async Task<ErrorOr<RoleBaseResponse>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(
            r => r.Id == request.Id,
            q => q.Include(r => r.Permissions),
            cancellationToken: cancellationToken);
        
        if (role == null)
        {
            return Error.NotFound(RoleErrors.NotFound, RoleErrors.NotFoundDescription);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            role.Name = request.Name;
        }
        
        foreach (var delPermission in request.DeletePermissions ?? [])
        {
            var permission = role.Permissions.FirstOrDefault(p => p.PermissionId == delPermission);
            if (permission != null)
            {
                role.Permissions.Remove(permission);
            }
        }
        
        foreach (var addPermission in request.Permissions ?? [])
        {
            var existingPermission = role.Permissions.FirstOrDefault(p => p.PermissionId == addPermission);
            if (existingPermission != null)
            {
                continue;
            }
            
            var permission = await permissionRepository.GetAsync(
                p => p.Id == addPermission, 
                cancellationToken: cancellationToken);

            if (permission != null)
            {
                role.Permissions.Add(new RolePermission
                {
                    Permission = permission,
                });
            }
        }
        
        var updatedRole = await roleRepository.UpdateAsync(role, cancellationToken);

        return new RoleBaseResponse
        {
            Id = role.Id,
            Name = updatedRole.Name!
        };
    }
}