using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Chatting;

public class ChatBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public string? LastMessage { get; set; }
}