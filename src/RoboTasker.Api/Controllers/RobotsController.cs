using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Models.Robots;
using RoboTasker.Application.Robots.Robots.CreateRobot;
using RoboTasker.Application.Robots.Robots.DeleteRobot;
using RoboTasker.Application.Robots.Robots.GetRobotById;
using RoboTasker.Application.Robots.Robots.GetRobots;
using RoboTasker.Application.Robots.Robots.UpdateRobot;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class RobotsController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateRobot(CreateRobotCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRobot(Guid id)
    {
        var command = new DeleteRobotCommand(id);
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllRobots([FromQuery] GetRobotsQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRobotById(Guid id)
    {
        var query = new GetRobotByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRobot(Guid id, UpdateRobotRequest request)
    {
        var command = request.Adapt<UpdateRobotCommand>();
        command.Id = id;
        
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }
}