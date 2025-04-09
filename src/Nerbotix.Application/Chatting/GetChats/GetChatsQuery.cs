using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Chatting.GetChats;

public class GetChatsQuery : ITenantQuery<PaginatedList<ChatBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}