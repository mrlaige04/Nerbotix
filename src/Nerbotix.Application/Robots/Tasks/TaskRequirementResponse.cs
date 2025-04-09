using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Robots.Tasks;

public class TaskRequirementResponse : EntityResponse
{
    public RobotTaskRequirementLevel Level { get; set; }
    public Guid CapabilityId { get; set; }
}