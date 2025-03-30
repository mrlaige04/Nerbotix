using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.Classical;

public class RoundRobinTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    private static int _lastAssignedIndex = -1; // TODO: move to tenant settings
    private static readonly object Lock = new(); 
    
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var availableRobots = await robots.ToListAsync();
        if (availableRobots.Count == 0)
        {
            return null;
        }

        var index = Interlocked.Increment(ref _lastAssignedIndex) % availableRobots.Count;
        return availableRobots[index];
    }
}