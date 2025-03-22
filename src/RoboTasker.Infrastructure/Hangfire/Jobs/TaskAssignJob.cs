using Hangfire;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Hangfire.Jobs;

public class TaskAssignJob(
    ITenantRepository<RobotTask> taskRepository)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task Execute(Guid taskId)
    {
        var task = await taskRepository.GetAsync(
            t => t.Id == taskId,
            q => q
                .Include(t => t.Archive)
                .Include(t => t.Requirements)
                .Include(t => t.AssignedRobot)
                .Include(t => t.TaskData));

        if (task is not { Status: RobotTaskStatus.Pending })
        {
            return;
        }
        
        task.Status = RobotTaskStatus.WaitingForEnqueue;
        await taskRepository.UpdateAsync(task);
        
        // TODO: Implement assignment here
    }
}