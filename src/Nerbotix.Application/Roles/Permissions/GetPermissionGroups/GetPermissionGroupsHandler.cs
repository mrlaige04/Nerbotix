using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Permissions.GetPermissionGroups;

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
                IsSystem = g.IsSystem,
                Name = g.Name,
            },
            cancellationToken: cancellationToken);

        return groups;
    }
}