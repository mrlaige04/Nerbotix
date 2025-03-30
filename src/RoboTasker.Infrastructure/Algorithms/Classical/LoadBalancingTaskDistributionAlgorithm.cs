using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.Classical;

public class LoadBalancingTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    private const double ComplexityFactor = 2.0; // TODO: Move to tenant settings
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var selectedRobot = await robots
            .OrderBy(r => r.TasksQueue.Sum(t => 
                (t.EstimatedDuration.HasValue 
                    ? (double?)t.EstimatedDuration!.Value.TotalSeconds
                    : 0)
                + (t.Complexity > 0 ? t.Complexity : 1) * ComplexityFactor)) 
            .ThenBy(r => r.LastActivity ?? DateTime.MinValue) 
            .FirstOrDefaultAsync();
        
        return selectedRobot;
    }
}