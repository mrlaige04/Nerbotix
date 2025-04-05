using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Chatting.DeleteChat;

public record DeleteChatCommand(Guid Id) : ITenantCommand;