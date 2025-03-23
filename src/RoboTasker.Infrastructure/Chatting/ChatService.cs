using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Chatting;
using RoboTasker.Application.Chatting.Models;
using RoboTasker.Domain.Chatting;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Infrastructure.Chatting;

public class ChatService(
    ITenantRepository<ChatMessage> messagesRepository,
    ITenantRepository<Chat> chatRepository) : IChatService
{
    public async Task<MessageResponse?> SaveMessage(IncomingMessage message)
    {
        var chat = await chatRepository.GetAsync(
            c => c.Id == message.ChatId && c.Users.Any(cu => cu.UserId == message.SenderId),
            q => q
                .Include(c => c.Messages)
                .Include(c => c.Users));

        if (chat == null)
        {
            return null;
        }

        var dbMessage = new ChatMessage
        {
            ChatId = message.ChatId,
            UserId = message.SenderId,
            IsSystem = false,
            Text = message.Message,
        };
        
        chat.Messages.Add(dbMessage);
        
        await chatRepository.UpdateAsync(chat);

        return new MessageResponse
        {
            Id = dbMessage.Id,
            CreatedAt = dbMessage.CreatedAt,
            IsSystem = dbMessage.IsSystem,
            SenderId = message.SenderId,
            Message = dbMessage.Text,
            UpdatedAt = dbMessage.UpdatedAt
        };
    }

    public async Task<bool> DeleteMessage(DeleteMessage message)
    {
        var msg = await messagesRepository.GetAsync(
            m => m.Id == message.MessageId && m.ChatId == message.ChatId);

        if (msg == null)
        {
            return false;
        }
        
        return await messagesRepository.DeleteAsync(msg);
    }
}