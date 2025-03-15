using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Roles.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : ITenantQuery<RoleResponse>;