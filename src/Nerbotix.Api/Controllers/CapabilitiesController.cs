using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Api.Models.Capabilities;
using Nerbotix.Application.Robots.Capabilities.CreateCapability;
using Nerbotix.Application.Robots.Capabilities.DeleteCapability;
using Nerbotix.Application.Robots.Capabilities.GetCapabilitiesGroups;
using Nerbotix.Application.Robots.Capabilities.GetCapabilitiesItems;
using Nerbotix.Application.Robots.Capabilities.GetCapabilityById;
using Nerbotix.Application.Robots.Capabilities.UpdateCapability;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
public class CapabilitiesController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateCapabilityGroup(CreateCapabilityCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCapability(Guid id)
    {
        var command = new DeleteCapabilityCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("groups")]
    public async Task<IActionResult> GetCapabilities([FromQuery] GetCapabilitiesGroupQuery groupQuery)
    {
        var result = await mediator.Send(groupQuery);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllCapabilities([FromQuery] GetCapabilitiesItemsQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCapability(Guid id)
    {
        var query = new GetCapabilityByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCapability(Guid id, UpdateCapabilityRequest request)
    {
        var command = request.Adapt<UpdateCapabilityCommand>();
        command.Id = id;
        
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}