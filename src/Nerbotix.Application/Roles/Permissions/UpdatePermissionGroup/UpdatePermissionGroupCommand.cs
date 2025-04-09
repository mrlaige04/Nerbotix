using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Roles.Permissions.CreatePermissionGroup;

namespace Nerbotix.Application.Roles.Permissions.UpdatePermissionGroup;

public class UpdatePermissionGroupCommand : ITenantCommand<PermissionGroupBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<CreatePermissionGroupItem>? Permissions { get; set; }
}