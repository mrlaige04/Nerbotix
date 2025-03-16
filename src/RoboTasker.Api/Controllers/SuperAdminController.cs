using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Application.SuperAdmin.Tenants.CreateTenant;

namespace RoboTasker.Api.Controllers;

[Route("[controller]")]
public class SuperAdminController(IMediator mediator) : BaseController
{
    [HttpPost("tenants")]
    public async Task<IActionResult> CreateTenant(CreateTenantCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}