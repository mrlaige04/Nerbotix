using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.GetCapabilitiesItems;

public class GetCapabilitiesItemsQuery : ITenantQuery<PaginatedList<CapabilityItemResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}