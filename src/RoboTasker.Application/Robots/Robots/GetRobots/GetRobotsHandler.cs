using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Robots.Categories;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Robots.GetRobots;

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
                }
            },
            include: q => q
                .Include(r => r.Category)
                .Include(r => r.Location),
            cancellationToken: cancellationToken);

        return robots;
    }
}