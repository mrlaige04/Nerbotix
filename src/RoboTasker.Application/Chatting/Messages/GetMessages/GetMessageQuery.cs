using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Chatting.Messages.GetMessages;

public class GetMessageQuery : ITenantQuery<PaginatedList<MessageResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid ChatId { get; set; }
}