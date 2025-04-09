namespace Nerbotix.Domain.Tenants.Settings.Algorithms;

public class TenantSimulatedAnnealingAlgorithmSettings
{
    public double InitialTemperature { get; set; }
    public double CoolingRate { get; set; }
    public int IterationsPerTemp { get; set; }
    public double MinTemperature { get; set; }
}