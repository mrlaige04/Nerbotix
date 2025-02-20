namespace RoboTasker.Domain.Tenants;

public class TenantUser
{
    public Tenant Tenant { get; set; } = null!;
    public Guid TenantId { get; set; }
    
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
}