using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Categories;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots.GetRobots;

public class GetRobotsHandler(
    ITenantRepository<Robot> robotRepository) : IQueryHandler<GetRobotsQuery, PaginatedList<RobotBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<RobotBaseResponse>>> Handle(GetRobotsQuery request, CancellationToken cancellationToken)
    {
        var robots = await robotRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            r => new RobotBaseResponse
            {
                Id = r.Id,
                Name = r.Name,
                TenantId = r.TenantId,
                Status = r.Status,
                Location = new RobotLocationResponse()
                {
                    Latitude = r.Location.Latitude,
                    Longitude = r.Location.Longitude,
                    Timestamp = r.Location.Timestamp.DateTime
                },
                Category = new CategoryBaseResponse
                {
                    Id = r.CategoryId,
                    Name = r.Category.Name,
                    TenantId = r.TenantId,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt,
                },
                LastActivity = r.LastActivity,
            },
            include: q => q
                .Include(r => r.Category)
                .Include(r => r.Location),
            cancellationToken: cancellationToken);

        return robots;
    }
}