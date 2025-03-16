using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Tenants;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Permissions.DeletePermissionGroup;

public class DeletePermissionGroupHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : ICommandHandler<DeletePermissionGroupCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeletePermissionGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await permissionGroupRepository.GetAsync(
            g => g.Id == request.Id,
            cancellationToken: cancellationToken);

        if (group == null)
        {
            return Error.NotFound(PermissionErrors.GroupNotFound, PermissionErrors.GroupNotFoundDescription);
        }

        if (group.IsSystem)
        {
            return Error.Forbidden(PermissionErrors.DeletingFailed, PermissionErrors.DeletionSystemGroupFailed);
        }
        
        await permissionGroupRepository.DeleteAsync(group, cancellationToken);

        return new Success();
    }
}