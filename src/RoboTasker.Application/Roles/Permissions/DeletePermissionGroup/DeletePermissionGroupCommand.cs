using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Permissions.DeletePermissionGroup;

public record DeletePermissionGroupCommand(Guid Id) : ITenantCommand;