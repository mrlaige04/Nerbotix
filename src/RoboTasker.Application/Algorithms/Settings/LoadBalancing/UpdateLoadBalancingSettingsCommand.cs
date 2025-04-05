using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Algorithms.Settings.LoadBalancing;

public class UpdateLoadBalancingSettingsCommand : ITenantCommand
{
    public double ComplexityFactor { get; set; }
}