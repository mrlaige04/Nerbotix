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
}