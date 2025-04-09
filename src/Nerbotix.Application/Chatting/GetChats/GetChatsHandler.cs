using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Chatting;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Application.Chatting.GetChats;

public class GetChatsHandler(
    ICurrentUser currentUser,
    ITenantRepository<Chat> chatRepository) : IQueryHandler<GetChatsQuery, PaginatedList<ChatBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<ChatBaseResponse>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.GetUserId();
        var chats = await chatRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            c => new ChatBaseResponse
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                LastMessage = c.Messages.Any() ? c.Messages.OrderByDescending(m => m.UpdatedAt).First().Text : null
            },
            c => c.Users.Any(cu => cu.UserId == userId),
            q => q.Include(c => c.Users),
            cancellationToken);

        return chats;
    }
}