using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Roles.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : ITenantQuery<RoleResponse>;