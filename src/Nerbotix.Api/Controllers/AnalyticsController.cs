using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Application.Analytics.GetActiveTasks;
using Nerbotix.Application.Analytics.GetMonthTasksCreatedCount;
using Nerbotix.Application.Analytics.GetRobotCountByCategories;
using Nerbotix.Application.Analytics.GetRobotStatuses;
using Nerbotix.Application.Analytics.GetTaskStatuses;

namespace Nerbotix.Api.Controllers;

[Route("[controller]")]
public class AnalyticsController(IMediator mediator) : BaseController
{
    [HttpGet("tasks-statuses")]
    public async Task<IActionResult> GetTaskStatuses()
    {
        var result = await mediator.Send(new GetTaskStatusesAnalyticQuery());
        return result.Match<IActionResult>(Ok, Problem);
    }
    
    [HttpGet("robot-statuses")]
    public async Task<IActionResult> GetRobotStatuses()
    {
        var result = await mediator.Send(new GetRobotStatusesAnalyticQuery());
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("month-tasks-created")]
    public async Task<IActionResult> GetMonthTasksCreated()
    {
        var result = await mediator.Send(new GetMonthTasksCreateCountAnalyticQuery());
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("active-tasks")]
    public async Task<IActionResult> GetActiveTasks([FromQuery] GetActiveTasksAnalyticQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("robot-by-categories")]
    public async Task<IActionResult> GetRobotCountByCategories()
    {
        var result = await mediator.Send(new GetRobotCountByCategoriesAnalyticQuery());
        return result.Match<IActionResult>(Ok, Problem);
    }
}