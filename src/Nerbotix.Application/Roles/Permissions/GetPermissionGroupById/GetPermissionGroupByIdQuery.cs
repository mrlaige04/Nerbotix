using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Permissions.GetPermissionGroupById;

public record GetPermissionGroupByIdQuery(Guid Id) : ITenantQuery<PermissionGroupResponse>;