namespace RoboTasker.Application.Algorithms.Settings;

public class SimulatedAnnealingSettingsResponse
{
    public double InitialTemperature { get; set; }
    public double CoolingRate { get; set; }
    public int IterationsPerTemp { get; set; }
    public double MinTemperature { get; set; }
}