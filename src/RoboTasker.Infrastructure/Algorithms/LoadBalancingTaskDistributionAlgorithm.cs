using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms;

public class LoadBalancingTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IList<Robot> robots)
    {
        var selectedRobot = robots
            .OrderBy(r => r.LastActivity ?? DateTime.MaxValue)
            .FirstOrDefault();
        
        return selectedRobot;
    }
}