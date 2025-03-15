using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Permissions.GetPermissionGroupById;

public record GetPermissionGroupByIdQuery(Guid Id) : ITenantQuery<PermissionGroupResponse>;