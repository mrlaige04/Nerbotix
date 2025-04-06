using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Chatting.GetChatInfo;
public record GetChatInfoQuery(Guid Id) : ITenantQuery<ChatInfoResponse>;