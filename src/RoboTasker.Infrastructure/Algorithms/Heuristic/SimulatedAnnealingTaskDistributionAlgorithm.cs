using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Infrastructure.Algorithms.Heuristic;

public class SimulatedAnnealingTaskDistributionAlgorithm(
    ITenantRepository<TenantSettings> settingsRepository) : ITaskDistributionAlgorithm
{
    private double InitialTemperature { get; set; }
    private double CoolingRate { get; set; }
    private int IterationsPerTemp { get; set; }
    private double MinTemperature { get; set; }
    
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var settings = await settingsRepository.GetAsync(
            t => t.TenantId == task.TenantId);

        if (settings == null)
        {
            return null;
        }

        var annealingSettings = settings.SimulatedAnnealingAlgorithmSettings;
        InitialTemperature = annealingSettings.InitialTemperature;
        CoolingRate = annealingSettings.CoolingRate;
        IterationsPerTemp = annealingSettings.IterationsPerTemp;
        MinTemperature = annealingSettings.MinTemperature;
        
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