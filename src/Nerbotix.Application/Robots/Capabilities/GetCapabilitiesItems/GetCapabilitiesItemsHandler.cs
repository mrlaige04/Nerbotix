using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.GetCapabilitiesItems;

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