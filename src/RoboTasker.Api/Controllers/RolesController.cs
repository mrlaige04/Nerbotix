
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Models.Tenants;
using RoboTasker.Application.Roles.Roles.CreateRole;
using RoboTasker.Application.Roles.Roles.DeleteRole;
using RoboTasker.Application.Roles.Roles.GetRoleById;
using RoboTasker.Application.Roles.Roles.GetRoles;
using RoboTasker.Application.Roles.Roles.UpdateRole;

namespace RoboTasker.Api.Controllers;

[Route("[controller]")]
public class RolesController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateRole(CreateRoleCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllRoles([FromQuery] GetRolesQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var query = new GetRoleByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(Guid id, UpdateRoleRequest request)
    {
        var command = request.Adapt<UpdateRoleCommand>();
        command.Id = id;
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}