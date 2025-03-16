using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Attributes;
using RoboTasker.Api.Models.Users;
using RoboTasker.Application.Users.CreateUser;
using RoboTasker.Application.Users.DeleteUser;
using RoboTasker.Application.Users.GetUserById;
using RoboTasker.Application.Users.GetUsers;
using RoboTasker.Application.Users.UpdateUser;
using RoboTasker.Domain.Consts;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class UsersController(IMediator mediator) : BaseController
{
    [HttpGet(""), Permission(PermissionNames.UsersRead)]
    public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}"), Permission(PermissionNames.UsersRead)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPost(""), Permission(PermissionNames.UsersCreate)]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}"), Permission(PermissionNames.UsersUpdate)]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateUserRequest request)
    {
        var command = request.Adapt<UpdateUserCommand>();
        command.Id = id;
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
    
    [HttpDelete("{id:guid}"), Permission(PermissionNames.UsersDelete)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var command = new DeleteUserCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}