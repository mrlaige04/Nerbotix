using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilitiesItems;

public class GetCapabilitiesItemsQuery : ITenantQuery<PaginatedList<CapabilityItemResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}