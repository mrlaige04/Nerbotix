using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Chatting;

public class ChatBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public string? LastMessage { get; set; }
}