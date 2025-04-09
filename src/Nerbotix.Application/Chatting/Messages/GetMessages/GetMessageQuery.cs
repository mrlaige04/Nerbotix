using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Chatting.Messages.GetMessages;

public class GetMessageQuery : ITenantQuery<PaginatedList<MessageResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid ChatId { get; set; }
}