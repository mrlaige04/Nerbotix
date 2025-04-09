using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Roles.Permissions.GetPermissionGroups;

public class GetPermissionGroupsQuery : ITenantQuery<PaginatedList<PermissionGroupBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}