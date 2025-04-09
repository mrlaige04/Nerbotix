using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Chatting;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Application.Chatting.CreateChat;

public class CreateChatHandler(
    ICurrentUser currentUser,
    ITenantRepository<Chat> chatRepository,
    ITenantRepository<Domain.Tenants.User> userRepository) : ICommandHandler<CreateChatCommand, ChatBaseResponse>
{
    public async Task<ErrorOr<ChatBaseResponse>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUser.GetUserId();
        var users = await userRepository.GetAllAsync(
            u => request.Users.Contains(u.Id),
            cancellationToken: cancellationToken);
        var usersNames = users.Select(u => u.UserName).ToList();

        var chatUsers = users
            .Select(u => new ChatUser
            {
                UserId = u.Id,
            })
            .ToList();
        
        chatUsers.Add(new ChatUser
        {
            UserId = currentUserId!.Value,
        });
        
        var chat = new Chat
        {
            Name = string.Join(", ", usersNames),
            Users = chatUsers,
        };

        if (!string.IsNullOrEmpty(request.Name))
        {
            chat.Name = request.Name;
        }
        
        var createdChat = await chatRepository.AddAsync(chat, cancellationToken);

        return new ChatBaseResponse
        {
            Id = createdChat.Id,
            Name = createdChat.Name,
            CreatedAt = createdChat.CreatedAt,
            UpdatedAt = createdChat.UpdatedAt
        };
    }
}