using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Application.Algorithms;

public interface ITaskDistributionAlgorithm
{
    Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots);
}