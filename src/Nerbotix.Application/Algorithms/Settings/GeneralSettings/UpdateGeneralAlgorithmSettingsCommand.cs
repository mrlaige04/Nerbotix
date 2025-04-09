using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Algorithms.Settings.GeneralSettings;

public class UpdateGeneralAlgorithmSettingsCommand : ITenantCommand
{
    public string Name { get; set; } = null!;
}