using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Chatting;

public class ChatMessage : TenantEntity
{
    public string Text { get; set; } = null!;
    
    public bool IsSystem { get; set; }
    
    public Guid? UserId { get; set; }
    
    public Chat Chat { get; set; } = null!;
    public Guid ChatId { get; set; }
}