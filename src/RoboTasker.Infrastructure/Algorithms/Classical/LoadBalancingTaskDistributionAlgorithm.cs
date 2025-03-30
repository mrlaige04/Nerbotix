using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Infrastructure.Algorithms.Classical;

public class LoadBalancingTaskDistributionAlgorithm(
    ITenantRepository<TenantSettings> settingsRepository) : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var settings = await settingsRepository.GetAsync(
            t => t.TenantId == task.TenantId);

        if (settings == null)
        {
            return null;
        }
        
        var complexityFactor = settings.LoadBalancingAlgorithmSettings.ComplexityFactor;
        
        var selectedRobot = await robots
            .OrderBy(r => r.TasksQueue.Sum(t => 
                (t.EstimatedDuration.HasValue 
                    ? (double?)t.EstimatedDuration!.Value.TotalSeconds
                    : 0)
                + (t.Complexity > 0 ? t.Complexity : 1) * complexityFactor)) 
            .ThenBy(r => r.LastActivity ?? DateTime.MinValue) 
            .FirstOrDefaultAsync();
        
        return selectedRobot;
    }
}