using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Tenants;

public class Permission : TenantEntity
{
    public string Name { get; set; } = null!;

    public bool IsSystem { get; set; }
    public PermissionGroup Group { get; set; } = null!;
    public Guid GroupId { get; set; }

    public IList<RolePermission> Roles { get; set; } = [];
}