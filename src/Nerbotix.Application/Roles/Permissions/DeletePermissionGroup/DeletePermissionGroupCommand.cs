using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Permissions.DeletePermissionGroup;

public record DeletePermissionGroupCommand(Guid Id) : ITenantCommand;