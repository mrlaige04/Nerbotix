using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Permissions.GetPermissionGroups;

public class GetPermissionGroupsHandler(
    ITenantRepository<PermissionGroup> permissionGroupRepository) : IQueryHandler<GetPermissionGroupsQuery, PaginatedList<PermissionGroupBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<PermissionGroupBaseResponse>>> Handle(GetPermissionGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await permissionGroupRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            g => new PermissionGroupBaseResponse
            {
                Id = g.Id,
                Name = g.Name,
            },
            cancellationToken: cancellationToken);

        return groups;
    }
}