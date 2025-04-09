using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Chatting.CreateChat;

public class CreateChatCommand : ITenantCommand<ChatBaseResponse>
{
    public string? Name { get; set; }
    public IList<Guid> Users { get; set; } = [];
}