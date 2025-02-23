using Microsoft.AspNetCore.Identity;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public sealed class User : IdentityUser<Guid>, IEntity<Guid>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    private User() { } // For EF 

    private User(string email)
    {
        Email = email;
        UserName = email;
        EmailConfirmed = true; 
    }

    public static User Create(string email) => new(email);

    public Tenant? Tenant { get; set; }
    public Guid? TenantId { get; set; }

    public IList<UserRole> Roles { get; set; } = [];
}