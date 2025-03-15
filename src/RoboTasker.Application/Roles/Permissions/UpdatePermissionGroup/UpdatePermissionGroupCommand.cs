using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Roles.Permissions.CreatePermissionGroup;

namespace RoboTasker.Application.Roles.Permissions.UpdatePermissionGroup;

public class UpdatePermissionGroupCommand : ITenantCommand<PermissionGroupBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<CreatePermissionGroupItem>? Permissions { get; set; }
}