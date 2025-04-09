using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Tenants;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Permissions.UpdatePermissionGroup;

public class UpdatePermissionGroupHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : ICommandHandler<UpdatePermissionGroupCommand, PermissionGroupBaseResponse>
{
    public async Task<ErrorOr<PermissionGroupBaseResponse>> Handle(UpdatePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await permissionGroupRepository.GetAsync(
            g => g.Id == request.Id,
            q => q
                .Include(g => g.Permissions),
            cancellationToken);
        
        if (group == null)
        {
            return Error.NotFound(PermissionErrors.GroupNotFound, PermissionErrors.GroupNotFoundDescription);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            group.Name = request.Name;
        }
        
        foreach (var delPermission in request.DeletePermissions ?? [])
        {
            var permission = group.Permissions.FirstOrDefault(p => p.Id == delPermission);
            if (permission == null) continue;
            if (permission.IsSystem)
            {
                return Error.Forbidden(PermissionErrors.DeletingFailed, PermissionErrors.DeletionSystemItemFailed);
            }

            group.Permissions.Remove(permission);
        }
        
        foreach (var newPermission in request.Permissions ?? [])
        {
            if (newPermission.ExistingId.HasValue)
            {
                var permission = group.Permissions.FirstOrDefault(p => p.Id == newPermission.ExistingId.Value);
                if (!string.IsNullOrEmpty(newPermission.Name) && permission != null)
                {
                    permission.Name = newPermission.Name;
                }
            }
            else
            {
                var permission = new Permission
                {
                    Name = newPermission.Name
                };
            
                group.Permissions.Add(permission);
            }
        }
        
        var updatedGroup = await permissionGroupRepository.UpdateAsync(group, cancellationToken);

        return new PermissionGroupBaseResponse
        {
            Id = updatedGroup.Id,
            Name = updatedGroup.Name,
            IsSystem = updatedGroup.IsSystem,
        };
    }
}