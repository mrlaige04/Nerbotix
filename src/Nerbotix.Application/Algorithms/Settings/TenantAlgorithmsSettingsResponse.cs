using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Algorithms.Settings;

public class TenantAlgorithmsSettingsResponse : TenantEntityResponse
{
    public string PreferredAlgorithm { get; set; } = null!;
    public AntColonySettingsResponse AntColony { get; set; } = null!;
    public GeneticSettingsResponse Genetic { get; set; } = null!;
    public LoadBalancingSettingsResponse LoadBalancing { get; set; } = null!;
    public SimulatedAnnealingSettingsResponse SimulatedAnnealing { get; set; } = null!;
}