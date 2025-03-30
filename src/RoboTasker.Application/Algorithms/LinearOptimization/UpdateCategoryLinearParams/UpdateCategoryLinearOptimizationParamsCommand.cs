using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Algorithms.LinearOptimization.UpdateCategoryLinearParams;

public class UpdateCategoryLinearOptimizationParamsCommand : ITenantCommand
{
    public Guid CategoryId { get; set; }
    public bool IsMaximization { get; set; }
    public IList<UpdateCategoryLinearOptimizationPropertyFactor> UpdateCategoryLinearParamsList { get; set; } = [];
}

public class UpdateCategoryLinearOptimizationPropertyFactor
{
    public Guid PropertyId { get; set; }
    public double Factor { get; set; }
}