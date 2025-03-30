using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.Classical;

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