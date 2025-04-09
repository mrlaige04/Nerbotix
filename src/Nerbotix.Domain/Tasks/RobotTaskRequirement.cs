using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Capabilities;

namespace Nerbotix.Domain.Tasks;

public class RobotTaskRequirement : TenantEntity
{
    public RobotTaskRequirementLevel RequiredLevel { get; set; } = RobotTaskRequirementLevel.Mandatory;
    
    public RobotTask Task { get; set; } = null!;
    public Guid TaskId { get; set; }

    public Capability Capability { get; set; } = null!;
    public Guid CapabilityId { get; set; }
}