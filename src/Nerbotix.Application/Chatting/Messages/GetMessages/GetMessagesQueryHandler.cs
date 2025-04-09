using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Chatting;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Chatting;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Chatting.Messages.GetMessages;

public class GetMessagesQueryHandler(
    ITenantRepository<Chat> chatRepository,
    ITenantRepository<ChatMessage> messageRepository) : IQueryHandler<GetMessageQuery, PaginatedList<MessageResponse>>
{
    public async Task<ErrorOr<PaginatedList<MessageResponse>>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
    {
        if (!await chatRepository.ExistsAsync(c => c.Id == request.ChatId, cancellationToken: cancellationToken))
        {
            return Error.NotFound(ChatErrors.NotFound, ChatErrors.NotFoundDescription);
        }
        
        var messages = await messageRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            m => new MessageResponse
            {
                Id = m.Id,
                Message = m.Text,
                IsSystem = m.IsSystem,
                SenderId = m.UserId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
            },
            m => m.ChatId == request.ChatId,
            cancellationToken: cancellationToken);

        return messages;
    }
}