using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Robots;

public abstract class RobotCommunication : TenantEntity
{
    public RobotCommunicationType Type { get; set; }

    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }
}