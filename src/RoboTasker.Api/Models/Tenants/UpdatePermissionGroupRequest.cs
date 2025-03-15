using RoboTasker.Application.Roles.Permissions.CreatePermissionGroup;

namespace RoboTasker.Api.Models.Tenants;

public class UpdatePermissionGroupRequest
{
    public string? Name { get; set; }
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<CreatePermissionGroupItem>? Permissions { get; set; }
}