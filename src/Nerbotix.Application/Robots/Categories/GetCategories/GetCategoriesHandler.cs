using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Categories.GetCategories;

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