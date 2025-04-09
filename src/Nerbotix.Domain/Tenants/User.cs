using Microsoft.AspNetCore.Identity;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Chatting;

namespace Nerbotix.Domain.Tenants;

public sealed class User : IdentityUser<Guid>, ITenantEntity<Guid>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public User() { } // For EF 

    private User(string email)
    {
        Email = email;
        UserName = email;
        EmailConfirmed = true; 
    }

    public static User Create(string email) => new(email);

    public Tenant Tenant { get; set; } = null!;
    public Guid TenantId { get; set; }

    public IList<ChatUser> Chats { get; set; } = [];
    
    public IList<UserRole> Roles { get; set; } = [];
}