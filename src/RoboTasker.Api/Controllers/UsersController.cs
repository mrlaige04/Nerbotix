using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Application.Users.CreateUser;
using RoboTasker.Application.Users.DeleteUser;
using RoboTasker.Application.Users.GetUsers;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class UsersController(IMediator mediator) : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }
}