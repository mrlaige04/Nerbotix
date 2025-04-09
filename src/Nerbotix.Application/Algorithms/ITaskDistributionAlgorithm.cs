using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Algorithms;

public interface ITaskDistributionAlgorithm
{
    Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots);
}