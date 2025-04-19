using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Infrastructure.Hangfire.Jobs;

public class GetNextTaskJob(
    IServiceProvider serviceProvider,
    ITenantRepository<Robot> robotRepository)
{
    [AutomaticRetry(Attempts = 0)]
    public async Task Execute(Guid robotId)
    {
        var robot = await robotRepository.GetAsync(
            r => r.Id == robotId,
            q => q
                .Include(r => r.TasksQueue)
                .Include(r => r.Communication));

        var task = robot?.TasksQueue
            .Where(t => 
                t.Status != RobotTaskStatus.Canceled && 
                t.Status != RobotTaskStatus.Completed && 
                t.Status != RobotTaskStatus.Failed)
            .OrderBy(t => t.Priority) // TODO: adjust to multi-criteria task (priority + complexity + estimated duration)
            .FirstOrDefault();

        if (task == null || robot == null)
        {
            return;
        }
        
        await using var scope = serviceProvider.CreateAsyncScope();
        var assigner = scope.ServiceProvider.GetRequiredKeyedService<ITaskAssignerService>(robot.Communication.Type);

        await assigner.AssignTask(task.Id);
    }
}