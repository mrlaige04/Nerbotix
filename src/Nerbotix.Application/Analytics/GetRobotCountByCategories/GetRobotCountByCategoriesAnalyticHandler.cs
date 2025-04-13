using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Analytics.GetRobotCountByCategories;

public class GetRobotCountByCategoriesAnalyticHandler(
    ITenantRepository<Robot> robotRepository) : IQueryHandler<GetRobotCountByCategoriesAnalyticQuery, GetRobotCountByCategoriesAnalyticResponse>
{
    public async Task<ErrorOr<GetRobotCountByCategoriesAnalyticResponse>> Handle(GetRobotCountByCategoriesAnalyticQuery request, CancellationToken cancellationToken)
    {
        var robots = await robotRepository.GetAllWithSelectorAsync(
            r => new { Category = r.Category.Name },
            include: q => q.Include(r => r.Category),
            cancellationToken: cancellationToken);

        var result = robots
            .GroupBy(r => r.Category)
            .Select(g => new GetRobotCountByCategoriesAnalyticResponseItem
            {
                CategoryName = g.Key,
                Count = g.Count()
            });

        return new GetRobotCountByCategoriesAnalyticResponse
        {
            Items = result.ToList()
        };
    }
}