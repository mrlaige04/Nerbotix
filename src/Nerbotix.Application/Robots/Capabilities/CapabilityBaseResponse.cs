using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities;

public class CapabilityBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = string.Empty;
    public string GroupName { get; set; } = null!;
    public string? Description { get; set; }
    public int CapabilitiesCount { get; set; }
}