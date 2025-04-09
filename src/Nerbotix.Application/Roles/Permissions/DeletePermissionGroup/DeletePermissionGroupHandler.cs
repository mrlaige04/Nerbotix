using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Tenants;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Permissions.DeletePermissionGroup;

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