using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Algorithms.Settings.GeneralSettings;

public class UpdateGeneralAlgorithmSettingsCommand : ITenantCommand
{
    public string Name { get; set; } = null!;
}