using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Nerbotix.Application.Chatting;
using Nerbotix.Application.Chatting.Models;
using Nerbotix.Domain.Services;

namespace Nerbotix.Infrastructure.Chatting;

[Authorize]
public class ChatHub(ICurrentUser currentUser, IChatService chatService) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var chatId = httpContext!.Request.Query["chatId"];
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
    }

    public async Task SendMessage(IncomingMessage message)
    {
        if (!Guid.TryParse(Context.User?.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return;
        }
        
        if (!Guid.TryParse(Context.User?.FindFirstValue("tenant-id"), out var tenantId))
        {
            return;
        }
        
        currentUser.SetTenantId(tenantId);
        message.SenderId = userId;
        var createdMessage = await chatService.SaveMessage(message);
        await Clients.Group(message.ChatId.ToString()).SendAsync("receive-message", createdMessage);
    }

    public async Task DeleteMessage(DeleteMessage message)
    {
        var result = await chatService.DeleteMessage(message);
        if (result)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("delete-message", message.MessageId);
        }
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var chatId = httpContext!.Request.Query["chatId"];
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
    }
}

public record TypingChange(Guid ChatId, Guid UserId);