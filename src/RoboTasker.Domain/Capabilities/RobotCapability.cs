using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Domain.Capabilities;

public class RobotCapability : TenantEntity
{
    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }
    
    public Capability Capability { get; set; } = null!;
    public Guid CapabilityId { get; set; }
}