using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Chatting;
using RoboTasker.Domain.Chatting;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;

namespace RoboTasker.Application.Chatting.DeleteChat;

public class DeleteChatHandler(
    ITenantRepository<Chat> chatRepository,
    ICurrentUser currentUser) : ICommandHandler<DeleteChatCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.GetUserId();
        var chat = await chatRepository.GetAsync(
            c => c.Id == request.Id,
            q => q.Include(c => c.Users),
            cancellationToken);

        if (chat == null)
        {
            return Error.NotFound(ChatErrors.NotFound, ChatErrors.NotFoundDescription);
        }
        
        var chatUser = chat.Users.FirstOrDefault(c => c.UserId == userId);
        if (chatUser != null)
        {
            chat.Users.Remove(chatUser);
        }

        if (chat.Users.Count == 0)
        {
            await chatRepository.DeleteAsync(chat, cancellationToken);
        }
        else
        {
            await chatRepository.UpdateAsync(chat, cancellationToken);
        }
        
        return new Success();
    }
}