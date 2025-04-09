using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Roles.DeleteRole;

public record DeleteRoleCommand(Guid Id) : ITenantCommand;