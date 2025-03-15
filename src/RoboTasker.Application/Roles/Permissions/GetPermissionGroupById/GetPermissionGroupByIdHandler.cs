using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Tenants;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Permissions.GetPermissionGroupById;

public class GetPermissionGroupByIdHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : IQueryHandler<GetPermissionGroupByIdQuery, PermissionGroupResponse>
{
    public async Task<ErrorOr<PermissionGroupResponse>> Handle(GetPermissionGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await permissionGroupRepository.GetWithSelectorAsync(
            g => new PermissionGroupResponse
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.Permissions
                    .Select(p => new PermissionBaseResponse
                    {
                        Id = p.Id,
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