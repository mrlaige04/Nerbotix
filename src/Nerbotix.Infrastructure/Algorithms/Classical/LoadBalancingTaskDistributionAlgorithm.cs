using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Algorithms;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Infrastructure.Algorithms.Classical;

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
        
        var complexityFactor = settings.AlgorithmSettings.LoadBalancingAlgorithmSettings.ComplexityFactor;
        
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