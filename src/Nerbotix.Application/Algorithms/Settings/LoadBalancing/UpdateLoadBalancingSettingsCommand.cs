using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Algorithms.Settings.LoadBalancing;

public class UpdateLoadBalancingSettingsCommand : ITenantCommand
{
    public double ComplexityFactor { get; set; }
}