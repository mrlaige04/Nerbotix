using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Tenants;
using Nerbotix.Application.Common.Extensions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Roles.DeleteRole;

public class DeleteRoleHandler(RoleManager<Role> roleManager) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            return Error.NotFound(RoleErrors.NotFound, RoleErrors.NotFoundDescription);
        }

        if (role.IsSystem)
        {
            return Error.Forbidden(RoleErrors.DeletingFailed, RoleErrors.DeletionSystemRoleFailed);
        }
        
        var result = await roleManager.DeleteAsync(role);
        return !result.Succeeded 
            ? result.ToErrorOr(RoleErrors.DeletingFailed, RoleErrors.DeletingFailedDescription) 
            : new Success();
    }
}