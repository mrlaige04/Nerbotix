using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilities;

public class GetCapabilitiesQuery : IQuery<PaginatedList<CapabilityBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}