using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Algorithms.Settings.AntColony;

public class UpdateAntColonySettingsCommand : ITenantCommand
{
    public int AntCount { get; set; }
    public int Iterations { get; set; }
    public double Evaporation { get; set; }
    public double Alpha { get; set; }
    public double Beta { get; set; }
}