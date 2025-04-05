using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Attributes;
using RoboTasker.Api.Controllers;
using RoboTasker.Application.SuperAdmin.Tenants.CreateTenant;
using RoboTasker.Application.SuperAdmin.Tenants.DeleteTenant;
using RoboTasker.Application.SuperAdmin.Tenants.GetTenants;
using RoboTasker.Domain.Consts;

namespace RoboTasker.Api.SuperAdmin;

[Route("sa/[controller]"), Authorize, OnlySuperAdmin]
public class TenantsController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<ActionResult> CreateTenant(CreateTenantCommand createTenantCommand)
    {
        var result = await mediator.Send(createTenantCommand);
        return result.Match(Ok, Problem);
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetTenants([FromQuery] GetTenantsQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTenant(Guid id)
    {
        var command = new DeleteTenantCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}