using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.MathBased;

public class AssignmentProblemTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var allRobots = await robots.ToListAsync();
        
        var costMatrix = BuildCostMatrix(allRobots);

        var assignments = SolveHungarianAlgorithm(costMatrix);

        var bestRobotIndex = assignments.IndexOf(assignments.Min());
        return allRobots[bestRobotIndex];
    }
    
    private static double[,] BuildCostMatrix(List<Robot> robots)
    {
        var size = robots.Count;
        var costMatrix = new double[size, size];

        for (var i = 0; i < size; i++)
        {
            costMatrix[i, i] = CalculateCost(robots[i]);
        }

        return costMatrix;
    }
    
    private static double CalculateCost(Robot robot)
    {
        double cost = 0;

        foreach (var prop in robot.Properties)
        {
            if (double.TryParse(prop.Value, out var val))
            {
                cost += val * prop.Property.Factor;
            }
        }

        cost += robot.TasksQueue.Count * 10; 

        return cost;
    }
    
    private List<int> SolveHungarianAlgorithm(double[,] costMatrix)
    {
        var size = costMatrix.GetLength(0);
        var assignments = new List<int>(new int[size]);

        var hungarian = new HungarianAlgorithm(costMatrix);
        var result = hungarian.Run();

        for (var i = 0; i < size; i++)
        {
            assignments[i] = result[i];
        }

        return assignments;
    }
}