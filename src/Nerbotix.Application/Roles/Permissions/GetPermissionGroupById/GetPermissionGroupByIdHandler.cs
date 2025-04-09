using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Tenants;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Permissions.GetPermissionGroupById;

public class GetPermissionGroupByIdHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : IQueryHandler<GetPermissionGroupByIdQuery, PermissionGroupResponse>
{
    public async Task<ErrorOr<PermissionGroupResponse>> Handle(GetPermissionGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await permissionGroupRepository.GetWithSelectorAsync(
            g => new PermissionGroupResponse
            {
                Id = g.Id,
                IsSystem = g.IsSystem,
                Name = g.Name,
                Permissions = g.Permissions
                    .Select(p => new PermissionBaseResponse
                    {
                        Id = p.Id,
                        IsSystem = p.IsSystem,
                        Name = p.Name,
                        GroupName = g.Name
                    })
                    .ToList()
            },
            g => g.Id == request.Id,
            q => q.Include(g => g.Permissions),
            cancellationToken: cancellationToken);

        if (group == null)
        {
            return Error.NotFound(PermissionErrors.GroupNotFound, PermissionErrors.GroupNotFoundDescription);
        }

        return group;
    }
}