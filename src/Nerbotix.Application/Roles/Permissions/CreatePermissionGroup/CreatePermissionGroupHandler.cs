using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Tenants;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Permissions.CreatePermissionGroup;

public class CreatePermissionGroupHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : ICommandHandler<CreatePermissionGroupCommand, PermissionGroupBaseResponse>
{
    public async Task<ErrorOr<PermissionGroupBaseResponse>> Handle(CreatePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        if (await permissionGroupRepository.ExistsAsync(
                g => EF.Functions.Like(g.Name, $"%{request.Name}%"), 
                cancellationToken: cancellationToken))
        {
            return Error.Conflict(PermissionErrors.ConflictGroup, PermissionErrors.ConflictGroupDescription);
        }
        
        var group = new PermissionGroup { Name = request.Name };

        foreach (var permission in request.Permissions)
        {
            var newPermission = new Permission
            {
                Name = permission.Name,
            };
            
            group.Permissions.Add(newPermission);
        }
        
        var createdGroup = await permissionGroupRepository.AddAsync(group, cancellationToken);

        return new PermissionGroupBaseResponse
        {
            Id = createdGroup.Id,
            Name = createdGroup.Name,
            IsSystem = createdGroup.IsSystem
        };
    }
}