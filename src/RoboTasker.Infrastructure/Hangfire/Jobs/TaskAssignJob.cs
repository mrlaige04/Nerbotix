using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Robots.Enums;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Hangfire.Jobs;

public class TaskAssignJob(
    IServiceProvider serviceProvider,
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

        if (task is not { Status: RobotTaskStatus.Pending or RobotTaskStatus.WaitingForEnqueue })
        {
            return;
        }
        
        task.Status = RobotTaskStatus.WaitingForEnqueue;
        await taskRepository.UpdateAsync(task);
        
        // TODO: Implement assignment here
        using var scope = serviceProvider.CreateScope();
        
        // TODO: implement selection from tenant settings
        var algorithm = scope.ServiceProvider.GetKeyedService<ITaskDistributionAlgorithm>(AlgorithmNames.LoadBalancing);
        
        var query = await robotRepository.GetQuery();

        var idleRobots = query
            .Include(r => r.Capabilities)
            .Include(r => r.Properties)
            .Include(c => c.CustomProperties)
            .Where(r => r.Status == RobotStatus.Idle && r.TenantId == task.TenantId && r.CategoryId == task.CategoryId);

        var requiredCapabilitiesIds = task.Requirements
            .Where(req => req.RequiredLevel == RobotTaskRequirementLevel.Mandatory)
            .Select(req => req.CapabilityId)
            .ToList();
        
        var capableRobots = idleRobots.Where(
            r => requiredCapabilitiesIds.All(
                c => r.Capabilities.Any(ca => ca.CapabilityId == c)));
        
        if (!await capableRobots.AnyAsync())
        {
            return;
        }
        
        var optionalCapabilitiesIds = task.Requirements
            .Where(req => req.RequiredLevel == RobotTaskRequirementLevel.Optional)
            .Select(req => req.CapabilityId)
            .ToList();

        var robotsWithOptionalCapabilities = capableRobots.Where(
            r => optionalCapabilitiesIds.Any(
                c => r.Capabilities.Any(ca => ca.CapabilityId == c)));
        
        /*var robotsWithOptionalCapabilities = capableRobots.Where(r =>
            task.Requirements
                .Where(req => req.RequiredLevel == RobotTaskRequirementLevel.Optional)
                .Any(req => r.Capabilities.Any(c => c.CapabilityId == req.CapabilityId))
        ).ToList();*/

        if (!await robotsWithOptionalCapabilities.AnyAsync())
        {
            robotsWithOptionalCapabilities = capableRobots;
        }

        var sourceRobots = robotsWithOptionalCapabilities;
        
        var robot = await algorithm!.FindRobot(task, sourceRobots.ToList());
        if (robot == null)
        {
            return;
        }
        
        task.AssignedRobotId = robot.Id;
        task.Status = RobotTaskStatus.InProgress;
        await taskRepository.UpdateAsync(task);
        
        robot.Status = RobotStatus.Busy;
        await taskRepository.UpdateAsync(task);
    }
}