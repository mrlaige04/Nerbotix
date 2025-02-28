using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public abstract class RobotCommunication : TenantEntity
{
    public RobotCommunicationType Type { get; set; }

    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }
}