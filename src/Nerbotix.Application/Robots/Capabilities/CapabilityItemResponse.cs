using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities;

public class CapabilityItemResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public string GroupName { get; set; } = null!;
    public string? Description { get; set; }
}