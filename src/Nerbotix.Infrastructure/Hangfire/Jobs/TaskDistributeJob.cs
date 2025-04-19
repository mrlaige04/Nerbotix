using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nerbotix.Application.Algorithms;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Algorithms;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Infrastructure.Hangfire.Jobs;

public class TaskDistributeJob(
    IServiceProvider serviceProvider,
    ITenantRepository<TenantSettings> tenantSettingsRepository,
    ITenantRepository<Robot> robotRepository,
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
        
        // TODO: Implement assignment here
        using var scope = serviceProvider.CreateScope();

        var algorithmName = await tenantSettingsRepository.GetWithSelectorAsync(
            a => a.AlgorithmSettings.PreferredAlgorithm,
            s => s.TenantId == task.TenantId);

        if (string.IsNullOrEmpty(algorithmName))
        {
            algorithmName = AlgorithmNames.Random;
        }
        
        // TODO: implement selection from tenant settings
        var algorithm = scope.ServiceProvider.GetKeyedService<ITaskDistributionAlgorithm>(algorithmName);
        
        var query = await robotRepository.GetQuery();

        var categoryRobots = query
            .Include(r => r.Capabilities)
            .Include(r => r.Properties)
                .ThenInclude(rp => rp.Property)
            .Include(c => c.CustomProperties)
            .Include(r => r.TasksQueue)
            .Include(r => r.Communication)
            .Where(r =>
                r.TenantId == task.TenantId && 
                r.CategoryId == task.CategoryId);

        if (!await categoryRobots.AnyAsync())
        {
            return;
        }
        
        var requiredCapabilitiesIds = task.Requirements
            .Where(req => req.RequiredLevel == RobotTaskRequirementLevel.Mandatory)
            .Select(req => req.CapabilityId)
            .ToList();
        
        var optionalCapabilitiesIds = task.Requirements
            .Where(req => req.RequiredLevel == RobotTaskRequirementLevel.Optional)
            .Select(req => req.CapabilityId)
            .ToList();
        
        var capableRobots = categoryRobots
            .Where(r => requiredCapabilitiesIds
                .All(c => r.Capabilities
                    .Any(ca => ca.CapabilityId == c)));
        
        if (!await capableRobots.AnyAsync())
        {
            return;
        }

        var robotsWithOptionalCapabilities = capableRobots
            .Where(r => optionalCapabilitiesIds
                .Any(c => r.Capabilities.Any(ca => ca.CapabilityId == c)));

        if (!await robotsWithOptionalCapabilities.AnyAsync())
        {
            robotsWithOptionalCapabilities = capableRobots;
        }

        var sourceRobots = robotsWithOptionalCapabilities;
        var robot = await algorithm!.FindRobot(task, sourceRobots);
        if (robot == null)
        {
            task.Status = RobotTaskStatus.Pending;
            await taskRepository.UpdateAsync(task);
            return;
        }
        
        var shouldStartImmediately = !robot.TasksQueue
            .Any(t => t.Status != RobotTaskStatus.Completed && t.Status != RobotTaskStatus.Failed);
        
        robot.TasksQueue.Add(task);
        await robotRepository.UpdateAsync(robot);
        
        task.AssignedRobotId = robot.Id;
        task.AssignedAt = DateTime.UtcNow;
        task.Status = RobotTaskStatus.WaitingForEnqueue;
        task.AssignedRobotName = robot.Name;
        await taskRepository.UpdateAsync(task);

        if (shouldStartImmediately)
        {
            var assigner = serviceProvider.GetRequiredKeyedService<ITaskAssignerService>(robot.Communication.Type);
            var assigned = await assigner.AssignTask(task.Id);

            if (assigned)
            {
                task.Status = RobotTaskStatus.InProgress;
                task.StartedAt = DateTime.UtcNow;
                await taskRepository.UpdateAsync(task);
                
                robot.LastActivity = DateTime.UtcNow;
                await robotRepository.UpdateAsync(robot);
            }
        }
    }
}