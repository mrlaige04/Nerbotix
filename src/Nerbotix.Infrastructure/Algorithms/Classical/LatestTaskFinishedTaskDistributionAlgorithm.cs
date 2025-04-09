using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Algorithms;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Infrastructure.Algorithms.Classical;

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