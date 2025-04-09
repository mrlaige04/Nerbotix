using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.GetCapabilitiesGroups;

public class GetCapabilitiesGroupQuery : ITenantQuery<PaginatedList<CapabilityBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}