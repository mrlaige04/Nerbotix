using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Algorithms;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Infrastructure.Algorithms.Classical;

public class RandomTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var availableRobots = await robots.ToListAsync();
        if (availableRobots.Count == 0)
        {
            return null;
        }
        
        var index = new Random().Next(0, availableRobots.Count);
        return availableRobots[index];
    }
}