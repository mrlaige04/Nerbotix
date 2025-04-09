using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Application.Chatting.CreateChat;
using Nerbotix.Application.Chatting.DeleteChat;
using Nerbotix.Application.Chatting.GetChatInfo;
using Nerbotix.Application.Chatting.GetChats;
using Nerbotix.Application.Chatting.Messages.GetMessages;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
public class ChatsController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateChat(CreateChatCommand command, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetChatInfo(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetChatInfoQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetChats([FromQuery] GetChatsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid id, [FromQuery] GetMessageQuery query, CancellationToken cancellation = default)
    {
        query.ChatId = id;
        var result = await mediator.Send(query, cancellation);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteChat(Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteChatCommand(id);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match(Ok, Problem);
    }
}