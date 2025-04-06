using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Infrastructure.Algorithms.Heuristic;

public class GeneticTaskDistributionAlgorithm(ITenantRepository<TenantSettings> settingsRepository) : ITaskDistributionAlgorithm
{
    private int PopulationSize { get; set; }
    private int Generations { get; set; }
    private double MutationRate { get; set; }
    
    public async Task<Robot?> FindRobot(RobotTask task, IQueryable<Robot> robots)
    {
        var settings = await settingsRepository.GetAsync(
            t => t.TenantId == task.TenantId);

        if (settings == null)
        {
            return null;
        }

        var geneticSettings = settings.AlgorithmSettings.GeneticAlgorithmSettings;
        PopulationSize = geneticSettings.PopulationSize;
        Generations = geneticSettings.Generations;
        MutationRate = geneticSettings.MutationRate;
        
        var allRobots = await robots.ToListAsync();

        var population = InitializePopulation(allRobots);

        for (var gen = 0; gen < geneticSettings.Generations; gen++)
        {
            var fitnessScores = population
                .Select(r => (robot: r, score: EvaluateFitness(r)))
                .ToList();
            
            var selected = fitnessScores
                .OrderByDescending(x => x.score)
                .Take(PopulationSize / 2)
                .Select(x => x.robot)
                .ToList();

            var offspring = Crossover(selected);
            
            Mutate(offspring, allRobots);
            
            population = selected.Concat(offspring).ToList();
        }
        
        return population.OrderByDescending(r => EvaluateFitness(r)).FirstOrDefault();
    }
    
    private List<Robot> InitializePopulation(List<Robot> allRobots)
    {
        return allRobots.OrderBy(_ => Guid.NewGuid()).Take(PopulationSize).ToList();
    }
    
    private static double EvaluateFitness(Robot robot)
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

        return score;
    }

    private List<Robot> Crossover(List<Robot> selected)
    {
        var offspring = new List<Robot>();
        var rnd = new Random();

        while (offspring.Count < PopulationSize / 2)
        {
            var parent1 = selected[rnd.Next(selected.Count)];
            var parent2 = selected[rnd.Next(selected.Count)];

            var child = new Robot
            {
                Name = parent1.Name + "-" + parent2.Name,
                Category = parent1.Category,
                CategoryId = parent1.CategoryId,
                Status = parent1.Status,
                LastActivity = parent1.LastActivity,
                Location = parent1.Location,
                LocationId = parent1.LocationId,
                Communication = parent1.Communication,
                TasksQueue = rnd.Next(2) == 0 ? parent1.TasksQueue : parent2.TasksQueue,
                Properties = rnd.Next(2) == 0 ? parent1.Properties : parent2.Properties
            };

            offspring.Add(child);
        }

        return offspring;
    }
    
    private void Mutate(List<Robot> offspring, List<Robot> allRobots)
    {
        var rnd = new Random();
        for (var i = 0; i < offspring.Count; i++)
        {
            if (rnd.NextDouble() < MutationRate)
            {
                offspring[i] = allRobots[rnd.Next(allRobots.Count)];
            }
        }
    }
}