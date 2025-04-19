using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.TestRobotApi.Jobs;
using Nerbotix.TestRobotApi.Models;

namespace Nerbotix.TestRobotApi.Controllers;

[ApiController, Route("tasks")]
public class TaskController(IBackgroundJobClient client) : ControllerBase
{
    [HttpPost("start")]
    public async Task<IActionResult> StartTask(StartTask task)
    {
        var jobId = client.Enqueue<ImitateWorkingJob>(job => job.Execute(task));
        return Ok(jobId);
    }
}