using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Chatting;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Chatting;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Chatting.Messages.GetMessages;

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