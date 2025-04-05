using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilitiesGroups;

public class GetCapabilitiesGroupQuery : ITenantQuery<PaginatedList<CapabilityBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}