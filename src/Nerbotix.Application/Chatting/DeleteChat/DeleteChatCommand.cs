using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Chatting.DeleteChat;

public record DeleteChatCommand(Guid Id) : ITenantCommand;