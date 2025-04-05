using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Algorithms.Settings.LinearOptimization;

public class UpdateLinearOptimizationSettingsHandler(
    ITenantRepository<RobotCategory> categoryRepository) : ICommandHandler<UpdateLinearOptimizationSettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateLinearOptimizationSettingsCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetAsync(
            c => c.Id == request.CategoryId,
            q => q.Include(c => c.Properties),
            cancellationToken);

        if (category == null)
        {
            return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }
        
        category.LinearOptimizationMaximization = request.IsMaximization;
        
        foreach (var param in request.UpdateCategoryLinearParamsList)
        {
            var property = category.Properties.FirstOrDefault(p => p.Id == param.PropertyId);
            if (property == null)
            {
                continue;
            }
            
            property.Factor = param.Factor;
        }
        
        await categoryRepository.UpdateAsync(category, cancellationToken);
        
        return new Success();   
    }
}