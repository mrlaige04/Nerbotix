using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Application.Common.Helpers;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Categories.UpdateCategory;

public class UpdateCategoryHandler(
    ITenantRepository<RobotCategory> categoryRepository,
    ITenantRepository<Robot> robotRepository) : ICommandHandler<UpdateCategoryCommand, CategoryBaseResponse>
{
    public async Task<ErrorOr<CategoryBaseResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetAsync(
            c => c.Id == request.Id,
            q => q
                .Include(c => c.Properties),
            cancellationToken: cancellationToken);
        
        if (category == null)
        {
            return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            category.Name = request.Name;
        }
        
        foreach (var deletePropertyId in request.DeletedProperties ?? [])
        {
            var property = category.Properties.FirstOrDefault(p => p.Id == deletePropertyId);
            if (property != null)
            {
                category.Properties.Remove(property);
            }
        }
        
        var newProperties = new List<RobotProperty>();
        foreach (var newProperty in request.NewProperties ?? [])
        {
            if (newProperty.ExistingId.HasValue)
            {
                var existingProperty = category.Properties
                    .FirstOrDefault(p => p.Id == newProperty.ExistingId.Value);

                if (existingProperty == null)
                {
                    continue;
                }
                
                existingProperty.Name = newProperty.Name;
                existingProperty.Type = newProperty.Type;
                existingProperty.Unit = newProperty.Unit;
            }
            else
            {
                var property = new RobotProperty
                {
                    Name = newProperty.Name,
                    Type = newProperty.Type,
                    CategoryId = category.Id,
                    Unit = newProperty.Unit,
                };
            
                newProperties.Add(property);
                category.Properties.Add(property);
            }
        }
        
        var updatedCategory = await categoryRepository.UpdateAsync(category, cancellationToken);
        
        var robots = await robotRepository.GetAllAsync(
            r => r.CategoryId == category.Id,
            q => q.Include(r => r.Properties),
            cancellationToken: cancellationToken);
        
        foreach (var newProp in newProperties)
        {
            foreach (var robot in robots)
            {
                robot.Properties.Add(new RobotPropertyValue
                {
                    Property = newProp,
                    Value = PropertyTypeHelper.GetDefaultValue(newProp.Type)
                });
            }
        }
        
        await robotRepository.UpdateRangeAsync(category.Robots, cancellationToken);
        
        return new CategoryBaseResponse
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            TenantId = updatedCategory.TenantId,
            CreatedAt = updatedCategory.CreatedAt,
            UpdatedAt = updatedCategory.UpdatedAt,
        };
    }
}