namespace Nerbotix.Domain.Tenants;

public class RolePermission
{
    public Role Role { get; set; } = null!;
    public Guid RoleId { get; set; }
    
    public Permission Permission { get; set; } = null!;
    public Guid PermissionId { get; set; }
}