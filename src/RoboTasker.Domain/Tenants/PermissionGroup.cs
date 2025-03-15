using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public class PermissionGroup : TenantEntity
{
    public string Name { get; set; } = null!;
    public IList<Permission> Permissions { get; set; } = [];
}