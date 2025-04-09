using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Chatting.GetChatInfo;
public record GetChatInfoQuery(Guid Id) : ITenantQuery<ChatInfoResponse>;