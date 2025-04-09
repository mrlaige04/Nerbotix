using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Categories.GetCategoryById;

public class GetCategoryByIdHandler(
    ITenantRepository<RobotCategory> categoriesRepository) : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<ErrorOr<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoriesRepository.GetWithSelectorAsync(
            c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                TenantId = c.TenantId,
                IsMaximization = c.LinearOptimizationMaximization == true,
                Properties = c.Properties
                    .Select(p => new CategoryPropertyResponse
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Type = p.Type,
                        Factor = p.Factor,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                    }).ToList(),
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            },
            c => c.Id == request.Id,
            q => q.Include(c => c.Properties),
            cancellationToken: cancellationToken);
        
        if (category == null)
        {
            return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }
        
        return category;
    }
}