using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Chatting;
using Nerbotix.Domain.Chatting;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Chatting.GetChatInfo;
public class GetChatInfoHandler(ITenantRepository<Chat> chatRepository) : IQueryHandler<GetChatInfoQuery, ChatInfoResponse>
{
    public async Task<ErrorOr<ChatInfoResponse>> Handle(GetChatInfoQuery request, CancellationToken cancellationToken)
    {
        var chat = await chatRepository.GetWithSelectorAsync(
            c => new ChatInfoResponse
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                UserCount = c.Users.Count,
                UpdatedAt = c.UpdatedAt,
                Name = c.Name
            },
            c => c.Id == request.Id,
            q => q.Include(c => c.Users),
            cancellationToken);

        if (chat is null)
        {
            return Error.NotFound(ChatErrors.NotFound, ChatErrors.NotFoundDescription);
        }

        return chat;
    }
}
