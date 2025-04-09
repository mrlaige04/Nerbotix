using Nerbotix.Application.Chatting.Models;

namespace Nerbotix.Application.Chatting;

public interface IChatService
{
    Task<MessageResponse?> SaveMessage(IncomingMessage message);
    Task<bool> DeleteMessage(DeleteMessage message);
}