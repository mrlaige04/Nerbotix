using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Categories.GetCategories;

public class GetCategoriesHandler(
    ITenantRepository<RobotCategory> categoriesRepository) : IQueryHandler<GetCategoriesQuery, PaginatedList<CategoryBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<CategoryBaseResponse>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await categoriesRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            c => new CategoryBaseResponse
            {
                Id = c.Id,
                Name = c.Name,
                TenantId = c.TenantId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            },
            cancellationToken: cancellationToken);
        
        return categories;
    }
}