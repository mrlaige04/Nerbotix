using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.MathBased;

public class FuzzyLogicTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var allRobots = await robots.ToListAsync();
        
        var robotScores = allRobots
            .Select(r => new { Robot = r, Score = EvaluateFuzzyLogic(r) })
            .OrderByDescending(r => r.Score)
            .ToList();
        
        return robotScores.FirstOrDefault()?.Robot;
    }
    
    private static double EvaluateFuzzyLogic(Robot robot)
    {
        var score = 1.0;

        foreach (var prop in robot.Properties)
        {
            if (!double.TryParse(prop.Value, out var val))
            {
                continue;
            }
            
            var membership = FuzzyMembership(val, prop.Property.Factor);
            score *= membership;
        }

        return score;
    }

    private static double FuzzyMembership(double value, double factor)
    {
        if (value < 0.3 * factor) return 0.2;
        if (value < 0.6 * factor) return 0.5;
        return 1.0;
    }
}