namespace Nerbotix.Application.Chatting.Models;

public class IncomingMessage
{
    public Guid ChatId { get; set; }
    public string Message { get; set; } = null!;
    public Guid SenderId { get; set; }
}