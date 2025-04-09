using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Capabilities;

public class Capability : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public CapabilityGroup Group { get; set; } = null!;
    public Guid GroupId { get; set; }

    public IList<RobotCapability> Robots { get; set; } = [];
}