using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.Classical;

public class LatestTaskFinishedTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var selectedRobot = await robots
            .OrderBy(r => r.LastActivity ?? DateTime.MinValue)
            .FirstOrDefaultAsync();
        
        return selectedRobot;
    }
}