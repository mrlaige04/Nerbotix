using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Capabilities;

public class Capability : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public CapabilityGroup Group { get; set; } = null!;
    public Guid GroupId { get; set; }

    public IList<RobotCapability> Robots { get; set; } = [];
}