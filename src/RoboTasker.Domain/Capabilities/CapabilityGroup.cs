using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Capabilities;

public class CapabilityGroup : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public IList<Capability> Capabilities { get; set; } = [];
}