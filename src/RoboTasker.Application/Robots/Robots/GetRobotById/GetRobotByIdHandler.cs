using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Application.Robots.Categories;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Robots.GetRobotById;

public class GetRobotByIdHandler(
    ITenantRepository<Robot> robotRepository) : IQueryHandler<GetRobotByIdQuery, RobotResponse>
{
    public async Task<ErrorOr<RobotResponse>> Handle(GetRobotByIdQuery request, CancellationToken cancellationToken)
    {
        var robot = await robotRepository.GetWithSelectorAsync(
            r => new RobotResponse
            {
                Id = r.Id,
                Name = r.Name,
                TenantId = r.TenantId,
                Category = new CategoryBaseResponse
                {
                    Id = r.CategoryId,
                    Name = r.Category.Name,
                    TenantId = r.Category.TenantId,
                },
                Properties = r.Properties
                    .Select(p => new RobotPropertyResponse
                    {
                        Id = p.Id,
                        PropertyId = p.PropertyId,
                        Name = p.Property.Name,
                        Value = p.Value.ToString()!,
                        Type = p.Property.Type,
                    }).ToList(),
                CustomProperties = r.CustomProperties
                    .Select(cp => new RobotCustomPropertyResponse()
                    {
                        Id = cp.Id,
                        Name = cp.Name,
                        Value = cp.Value.ToString()!,
                        TenantId = cp.TenantId,
                        CreatedAt = cp.CreatedAt,
                        UpdatedAt = cp.UpdatedAt,
                    }).ToList(),
                Capabilities = r.Capabilities
                    .Select(c => new RobotCapabilityResponse
                    {
                        Id = c.CapabilityId,
                        GroupId = c.Capability.GroupId,
                    }).ToList(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            },
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Properties)
                    .ThenInclude(p => p.Property)
                .Include(r => r.CustomProperties)
                .Include(r => r.Category)
                .Include(r => r.Capabilities)
                    .ThenInclude(c => c.Capability),
            cancellationToken);

        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }

        return robot;
    }
}