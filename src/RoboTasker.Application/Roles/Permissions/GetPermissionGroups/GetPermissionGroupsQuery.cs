using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Roles.Permissions.GetPermissionGroups;

public class GetPermissionGroupsQuery : ITenantQuery<PaginatedList<PermissionGroupBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}