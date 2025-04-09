using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Api.Models.Tenants;
using Nerbotix.Application.Roles.Permissions.CreatePermissionGroup;
using Nerbotix.Application.Roles.Permissions.DeletePermissionGroup;
using Nerbotix.Application.Roles.Permissions.GetPermissionGroupById;
using Nerbotix.Application.Roles.Permissions.GetPermissionGroups;
using Nerbotix.Application.Roles.Permissions.UpdatePermissionGroup;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
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