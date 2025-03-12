using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Api.Models.Tasks;
using RoboTasker.Application.Robots.Tasks.CreateTask;
using RoboTasker.Application.Robots.Tasks.DeleteTask;
using RoboTasker.Application.Robots.Tasks.GetTaskById;
using RoboTasker.Application.Robots.Tasks.GetTasks;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class TasksController(IMediator mediator) : BaseController
{
    [HttpPost(""), DisableRequestSizeLimit]
    public async Task<IActionResult> CreateTask([FromForm] CreateTaskRequest request)
    {
        var command = request.Adapt<CreateTaskCommand>();
        command.Files = request.Files;
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetTasksQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var query = new GetTaskByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var command = new DeleteTaskCommand(id);
        var result = await mediator.Send(command);
        return result.Match<IActionResult>(Ok, Problem);
    }
}