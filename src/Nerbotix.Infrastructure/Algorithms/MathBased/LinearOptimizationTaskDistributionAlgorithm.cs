using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Algorithms;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Infrastructure.Algorithms.MathBased;

public class LinearOptimizationTaskDistributionAlgorithm(
    ITenantRepository<RobotCategory> categoriesRepository) : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var allRobots = await robots.ToListAsync();

        var category = await categoriesRepository.GetAsync(
            c => c.Id == task.CategoryId);

        if (category == null)
        {
            return null;
        }
        
        var robotsWithScores = allRobots
            .Select(r => new
            {
                Robot = r,
                Score = r.Properties
                    .Where(pv =>
                        pv.Property.Type == RobotPropertyType.Number && double.TryParse(pv.Value.ToString(), out _))
                    .Sum(pv => double.Parse(pv.Value) * pv.Property.Factor)
            });

        var maximize = category.LinearOptimizationMaximization == true;
        
        var selectedRobot = maximize 
            ? robotsWithScores.OrderByDescending(robot => robot.Score).FirstOrDefault()
            : robotsWithScores.OrderBy(robot => robot.Score).FirstOrDefault();
        
        return selectedRobot?.Robot;
    }
}