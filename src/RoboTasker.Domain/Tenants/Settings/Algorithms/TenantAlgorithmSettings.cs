using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Algorithms;

namespace RoboTasker.Domain.Tenants.Settings.Algorithms;

public class TenantAlgorithmSettings : TenantEntity
{
    public string PreferredAlgorithm { get; set; } = null!;
    public TenantLoadBalancingAlgorithmSettings LoadBalancingAlgorithmSettings { get; set; } = null!;
    public TenantGeneticAlgorithmSettings GeneticAlgorithmSettings { get; set; } = null!;
    public TenantAntColonyAlgorithmSettings AntColonyAlgorithmSettings { get; set; } = null!;
    public TenantSimulatedAnnealingAlgorithmSettings SimulatedAnnealingAlgorithmSettings { get; set; } = null!;
    
    public static TenantAlgorithmSettings CreateDefault(Guid tenantId)
    {
        return new TenantAlgorithmSettings
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