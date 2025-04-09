using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Api.Attributes;
using Nerbotix.Api.Models.Users;
using Nerbotix.Application.Users.CreateUser;
using Nerbotix.Application.Users.DeleteUser;
using Nerbotix.Application.Users.GetUserById;
using Nerbotix.Application.Users.GetUsers;
using Nerbotix.Application.Users.UpdateUser;
using Nerbotix.Domain.Consts;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
public class UsersController(IMediator mediator) : BaseController
{
    [HttpGet(""), Permission(PermissionCombining.AtLeastOne, 
         PermissionNames.UsersRead, PermissionNames.ChatCreate)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}"), Permission(permissions: PermissionNames.UsersRead)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost(""), Permission(permissions: PermissionNames.UsersCreate)]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}"), Permission(permissions: PermissionNames.UsersUpdate)]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest request)
    {
        var command = request.Adapt<UpdateUserCommand>();
        command.Id = id;
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
    
    [HttpDelete("{id:guid}"), Permission(permissions: PermissionNames.UsersDelete)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}