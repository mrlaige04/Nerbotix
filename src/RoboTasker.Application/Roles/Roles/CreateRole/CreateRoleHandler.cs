using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Tenants;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Roles.CreateRole;

public class CreateRoleHandler(
    ITenantRepository<Role> roleRepository,
    ITenantRepository<Permission> permissionRepository) : ICommandHandler<CreateRoleCommand, RoleBaseResponse>
{
    public async Task<ErrorOr<RoleBaseResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await roleRepository.ExistsAsync(
                r => EF.Functions.Like(r.Name, $"%{request.Name}%"), 
                cancellationToken: cancellationToken))
        {
            return Error.Conflict(RoleErrors.Conflict, RoleErrors.ConflictDescription);
        }
        
        var role = new Role
        {
            Name = request.Name
        };
        
        foreach (var addPermission in request.Permissions)
        {
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
        
        var createdRole = await roleRepository.AddAsync(role, cancellationToken);
        
        return new RoleBaseResponse
        {
            Name = createdRole.Name,
            IsSystem = createdRole.IsSystem
        };
    }
}