using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Algorithms;

namespace RoboTasker.Domain.Tenants.Settings;

public class TenantSettings : TenantEntity
{
    public string PreferredAlgorithm = AlgorithmNames.Random;

    public TenantLoadBalancingAlgorithmSettings LoadBalancingAlgorithmSettings { get; set; } = new();
    public TenantGeneticAlgorithmSettings GeneticAlgorithmSettings { get; set; } = new();
    public TenantAntColonyAlgorithmSettings AntColonyAlgorithmSettings { get; set; } = new();
    public TenantSimulatedAnnealingAlgorithmSettings SimulatedAnnealingAlgorithmSettings { get; set; } = new();

    public static TenantSettings CreateDefault(Guid tenantId)
    {
        return new TenantSettings
        {
            TenantId = tenantId,
            PreferredAlgorithm = AlgorithmNames.Random,
            LoadBalancingAlgorithmSettings = new TenantLoadBalancingAlgorithmSettings
            {
                ComplexityFactor = 1,
            },
            GeneticAlgorithmSettings = new TenantGeneticAlgorithmSettings
            {
                Generations = 1,
                MutationRate = 1,
                PopulationSize = 1,
            },
            AntColonyAlgorithmSettings = new TenantAntColonyAlgorithmSettings
            {
                Alpha = 1,
                Beta = 1,
                Evaporation = 1,
                Iterations = 1,
                AntCount = 1
            },
            SimulatedAnnealingAlgorithmSettings = new TenantSimulatedAnnealingAlgorithmSettings
            {
                CoolingRate = 1,
                InitialTemperature = 1,
                MinTemperature = 1,
                IterationsPerTemp = 1
            }
        };
    }
}