using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Algorithms.Heuristic;

public class SimulatedAnnealingTaskDistributionAlgorithm : ITaskDistributionAlgorithm
{
    private const double InitialTemperature = 1000.0;
    private const double CoolingRate = 0.95;
    private const int IterationsPerTemp = 10;
    private const double MinTemperature = 0.1;
    
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var allRobots = await robots.ToListAsync();
        
        var random = new Random();
        var currentRobot = allRobots[random.Next(allRobots.Count)];
        var bestRobot = currentRobot;
        var currentFitness = EvaluateFitness(currentRobot, task);
        var bestFitness = currentFitness;
        
        var temperature = InitialTemperature;

        while (temperature > MinTemperature)
        {
            for (var i = 0; i < IterationsPerTemp; i++)
            {
                var neighbor = allRobots[random.Next(allRobots.Count)];
                var neighborFitness = EvaluateFitness(neighbor, task);

                if (!(neighborFitness > currentFitness) &&
                    !AcceptWorseSolution(currentFitness, neighborFitness, temperature))
                {
                    continue;
                }
                
                currentRobot = neighbor;
                currentFitness = neighborFitness;

                if (!(currentFitness > bestFitness))
                {
                    continue;
                }
                
                bestRobot = currentRobot;
                bestFitness = currentFitness;
            }

            temperature *= CoolingRate;
        }

        return bestRobot;
    }
    
    private static double EvaluateFitness(Robot robot, RobotTask task)
    {
        double score = 0;

        foreach (var prop in robot.Properties)
        {
            if (double.TryParse(prop.Value, out var val))
            {
                score += val * prop.Property.Factor;
            }
        }

        score -= robot.TasksQueue.Count * 10;
        score -= robot.TasksQueue.Sum(t => t.EstimatedDuration!.Value.TotalSeconds) / 60.0;

        return Math.Max(score, 0.1);
    }

    private static bool AcceptWorseSolution(double currentFitness, double newFitness, double temperature)
    {
        if (newFitness > currentFitness) return true;

        var probability = Math.Exp((newFitness - currentFitness) / temperature);
        return new Random().NextDouble() < probability;
    }
}