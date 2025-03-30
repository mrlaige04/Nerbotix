using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Algorithms.Settings.LinearOptimization;

public class UpdateLinearOptimizationSettingsCommand : ITenantCommand
{
    public Guid CategoryId { get; set; }
    public bool IsMaximization { get; set; }
    public IList<UpdateLinearOptimizationProperty> UpdateCategoryLinearParamsList { get; set; } = [];
}

public class UpdateLinearOptimizationProperty
{
    public Guid PropertyId { get; set; }
    public double Factor { get; set; }
}