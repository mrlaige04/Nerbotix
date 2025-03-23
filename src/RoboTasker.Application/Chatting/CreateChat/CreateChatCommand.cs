using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Chatting.CreateChat;

public class CreateChatCommand : ITenantCommand<ChatBaseResponse>
{
    public string? Name { get; set; }
    public IList<Guid> Users { get; set; } = [];
}