using Microsoft.AspNetCore.Identity;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Tenants;

public class Role : IdentityRole<Guid>, ITenantEntity<Guid>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public bool IsSystem { get; set; }
    
    public IList<UserRole> UserRoles { get; set; } = [];
    public IList<RolePermission> Permissions { get; set; } = [];
    public Tenant Tenant { get; set; } = null!;
    public Guid TenantId { get; set; }
}