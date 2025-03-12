using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilitiesItems;

public class GetCapabilitiesItemsHandler(
    ITenantRepository<Capability> capabilityRepository) : IQueryHandler<GetCapabilitiesItemsQuery, PaginatedList<CapabilityItemResponse>>
{
    public async Task<ErrorOr<PaginatedList<CapabilityItemResponse>>> Handle(GetCapabilitiesItemsQuery request, CancellationToken cancellationToken)
    {
        var capabilities = await capabilityRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            c => new CapabilityItemResponse
            {
                Id = c.Id,
                Name = c.Name,
                GroupName = c.Group.Name,
                Description = c.Description,
            },
            include: q => q.Include(c => c.Group),
            cancellationToken: cancellationToken);
        
        return capabilities;
    }
}