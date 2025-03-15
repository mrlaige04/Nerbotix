using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Models.Tenants;
using RoboTasker.Application.Roles.Permissions.CreatePermissionGroup;
using RoboTasker.Application.Roles.Permissions.DeletePermissionGroup;
using RoboTasker.Application.Roles.Permissions.GetPermissionGroupById;
using RoboTasker.Application.Roles.Permissions.GetPermissionGroups;
using RoboTasker.Application.Roles.Permissions.UpdatePermissionGroup;

namespace RoboTasker.Api.Controllers;

[Route("[controller]")]
public class PermissionsController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateGroup(CreatePermissionGroupCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllPermissionGroups([FromQuery] GetPermissionGroupsQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPermissionGroupById(Guid id)
    {
        var query = new GetPermissionGroupByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePermissionGroupById(Guid id, UpdatePermissionGroupRequest request)
    {
        var command = request.Adapt<UpdatePermissionGroupCommand>();
        command.Id = id;
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteGroup(Guid id)
    {
        var command = new DeletePermissionGroupCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}