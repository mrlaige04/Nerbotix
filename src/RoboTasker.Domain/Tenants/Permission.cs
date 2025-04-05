using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public class Permission : TenantEntity
{
    public string Name { get; set; } = null!;

    public bool IsSystem { get; set; }
    public PermissionGroup Group { get; set; } = null!;
    public Guid GroupId { get; set; }

    public IList<RolePermission> Roles { get; set; } = [];
}