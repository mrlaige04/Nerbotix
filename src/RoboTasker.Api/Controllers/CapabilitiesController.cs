using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Models.Capabilities;
using RoboTasker.Application.Robots.Capabilities.CreateCapability;
using RoboTasker.Application.Robots.Capabilities.DeleteCapability;
using RoboTasker.Application.Robots.Capabilities.GetCapabilities;
using RoboTasker.Application.Robots.Capabilities.GetCapabilityById;
using RoboTasker.Application.Robots.Capabilities.UpdateCapability;

namespace RoboTasker.Api.Controllers;

[Route("[controller]")]
public class CapabilitiesController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateCapabilityGroup(CreateCapabilityCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCapability(Guid id)
    {
        var command = new DeleteCapabilityCommand(id);
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetCapabilities([FromQuery] GetCapabilitiesQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapability(Guid id)
    {
        var query = new GetCapabilityByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCapability(Guid id, UpdateCapabilityRequest request)
    {
        var command = request.Adapt<UpdateCapabilityCommand>();
        command.Id = id;
        
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }
}