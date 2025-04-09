using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Tenants;

public class PermissionGroup : TenantEntity
{
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
    public IList<Permission> Permissions { get; set; } = [];
}