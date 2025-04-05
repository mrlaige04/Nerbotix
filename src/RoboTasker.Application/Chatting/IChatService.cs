using RoboTasker.Application.Chatting.Models;

namespace RoboTasker.Application.Chatting;

public interface IChatService
{
    Task<MessageResponse?> SaveMessage(IncomingMessage message);
    Task<bool> DeleteMessage(DeleteMessage message);
}