using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilities;

public class GetCapabilitiesHandler(
    ITenantRepository<CapabilityGroup> capabilityGroupRepository) : IQueryHandler<GetCapabilitiesQuery, PaginatedList<CapabilityBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<CapabilityBaseResponse>>> Handle(GetCapabilitiesQuery request, CancellationToken cancellationToken)
    {
        var capabilities = await capabilityGroupRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            c => new CapabilityBaseResponse
            {
                Id = c.Id,
                GroupName = c.Name,
                Description = c.Description,
                CapabilitiesCount = c.Capabilities.Count,
                TenantId = c.TenantId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            },
            include: q => q.Include(c => c.Capabilities),
            cancellationToken: cancellationToken);
        
        return capabilities;
    }
}