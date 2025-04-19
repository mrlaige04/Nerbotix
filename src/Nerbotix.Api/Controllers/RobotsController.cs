using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Api.Attributes;
using Nerbotix.Api.Models.Robots;
using Nerbotix.Application.Robots.Robots.CreateRobot;
using Nerbotix.Application.Robots.Robots.DeleteRobot;
using Nerbotix.Application.Robots.Robots.GetNextTask;
using Nerbotix.Application.Robots.Robots.GetRobotById;
using Nerbotix.Application.Robots.Robots.GetRobots;
using Nerbotix.Application.Robots.Robots.UpdateRobot;
using Nerbotix.Application.Robots.Robots.UpdateStatus;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
public class RobotsController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateRobot(CreateRobotCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRobot(Guid id)
    {
        var command = new DeleteRobotCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllRobots([FromQuery] GetRobotsQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRobotById(Guid id)
    {
        var query = new GetRobotByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateRobotStatus(Guid id, UpdateStatusCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}/next-task")]
    public async Task<IActionResult> GetNextTask(Guid id)
    {
        var command = new GetNextTaskCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRobot(Guid id, UpdateRobotRequest request)
    {
        var command = request.Adapt<UpdateRobotCommand>();
        command.Id = id;
        
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}