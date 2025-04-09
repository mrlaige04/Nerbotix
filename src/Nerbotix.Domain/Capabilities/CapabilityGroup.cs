using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Capabilities;

public class CapabilityGroup : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public IList<Capability> Capabilities { get; set; } = [];
}