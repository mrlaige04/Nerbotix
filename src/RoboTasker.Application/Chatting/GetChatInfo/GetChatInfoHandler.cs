using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Chatting;
using RoboTasker.Domain.Chatting;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Chatting.GetChatInfo;
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
