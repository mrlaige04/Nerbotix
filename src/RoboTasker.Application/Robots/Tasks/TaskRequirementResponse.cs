using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Application.Robots.Tasks;

public class TaskRequirementResponse : EntityResponse
{
    public RobotTaskRequirementLevel Level { get; set; }
    public Guid CapabilityId { get; set; }
}