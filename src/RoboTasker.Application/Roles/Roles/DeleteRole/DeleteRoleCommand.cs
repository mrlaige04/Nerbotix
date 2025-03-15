using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Roles.DeleteRole;

public record DeleteRoleCommand(Guid Id) : ITenantCommand;