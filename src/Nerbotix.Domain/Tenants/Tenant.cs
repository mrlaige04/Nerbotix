using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Chatting;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Domain.Tenants;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IList<User> Users { get; set; } = [];
    public IList<Role> Roles { get; set; } = [];
    public IList<PermissionGroup> PermissionGroups { get; set; } = [];
    public IList<Chat> Chats { get; set; } = [];
    
    public TenantSettings Settings { get; set; } = null!;
}