using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities;

public class CapabilityItemResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}