using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Algorithms.Settings.Genetic;

public class UpdateGeneticSettingsCommand : ITenantCommand
{
    public int PopulationSize { get; set; }
    public int Generations { get; set; }
    public double MutationRate { get; set; }
}