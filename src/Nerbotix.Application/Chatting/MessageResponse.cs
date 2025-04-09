using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Chatting;

public class MessageResponse : TenantEntityResponse
{
    public bool IsSystem { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? SenderId { get; set; }
}