using Nerbotix.Application.Roles.Permissions.CreatePermissionGroup;

namespace Nerbotix.Api.Models.Tenants;

public class UpdatePermissionGroupRequest
{
    public string? Name { get; set; }
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<CreatePermissionGroupItem>? Permissions { get; set; }
}