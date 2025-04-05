using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public class UserRole : TenantEntity
{
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    
    public Role Role { get; set; } = null!;
    public Guid RoleId { get; set; }
    
    public Tenant Tenant { get; set; } = null!;
}