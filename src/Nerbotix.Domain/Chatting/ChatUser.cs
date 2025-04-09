using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Domain.Chatting;

public class ChatUser : TenantEntity
{
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    
    public Chat Chat { get; set; } = null!;
    public Guid ChatId { get; set; }
}