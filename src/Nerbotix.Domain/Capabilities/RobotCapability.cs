using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Domain.Capabilities;

public class RobotCapability : TenantEntity
{
    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }
    
    public Capability Capability { get; set; } = null!;
    public Guid CapabilityId { get; set; }
}