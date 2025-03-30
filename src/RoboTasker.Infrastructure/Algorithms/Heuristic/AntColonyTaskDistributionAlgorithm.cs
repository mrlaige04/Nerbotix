using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Infrastructure.Algorithms.Heuristic;

public class AntColonyTaskDistributionAlgorithm(ITenantRepository<TenantSettings> settingsRepository) : ITaskDistributionAlgorithm
{
    private int AntCount { get; set; }
    private int Iterations { get; set; }
    private double Evaporation { get; set; }
    private double Alpha { get; set; }
    private double Beta { get; set; }
    
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var settings = await settingsRepository.GetAsync(
            t => t.TenantId == task.TenantId);

        if (settings == null)
        {
            return null;
        }

        var antSettings = settings.AntColonyAlgorithmSettings;
        AntCount = antSettings.AntCount;
        Iterations = antSettings.Iterations;
        Evaporation = antSettings.Evaporation;
        Alpha = antSettings.Alpha;
        Beta = antSettings.Beta;
        
        var allRobots = await robots.ToListAsync();

        var pheromones = allRobots.ToDictionary(r => r, _ => 1.0);

        for (var iter = 0; iter < Iterations; iter++)
        {
            var antPaths = new List<Robot>();

            for (var ant = 0; ant < AntCount; ant++)
            {
                var chosenRobot = SelectRobot(allRobots, pheromones, task);
                if (chosenRobot != null)
                {
                    antPaths.Add(chosenRobot);
                }
            }
            
            UpdatePheromones(pheromones, antPaths);
        }
        
        return pheromones.OrderByDescending(x => x.Value).FirstOrDefault().Key;
    }
    
    private Robot? SelectRobot(List<Robot> robots, Dictionary<Robot, double> pheromones, RobotTask task)
    {
        var probabilities = new Dictionary<Robot, double>();
        double sum = 0;

        foreach (var robot in robots)
        {
            var heuristic = EvaluateFitness(robot, task);
            var pheromone = Math.Pow(pheromones[robot], Alpha) * Math.Pow(heuristic, Beta);
            probabilities[robot] = pheromone;
            sum += pheromone;
        }

        if (sum == 0) return null;

        var rand = new Random().NextDouble() * sum;
        double cumulative = 0;

        foreach (var (robot, probability) in probabilities)
        {
            cumulative += probability;
            if (cumulative >= rand) return robot;
        }

        return null;
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

    private void UpdatePheromones(Dictionary<Robot, double> pheromones, List<Robot> antPaths)
    {
        foreach (var robot in pheromones.Keys.ToList())
        {
            pheromones[robot] *= 1 - Evaporation;
        }

        foreach (var robot in antPaths)
        {
            pheromones[robot] += 1.0;
        }
    }
}