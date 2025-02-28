using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public class Robot : TenantEntity
{
    public string Name { get; set; } = null!;
    
    public RobotCategory Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
    
    public RobotCommunication Communication { get; set; } = null!;

    public IList<RobotPropertyValue> Properties { get; set; } = [];
    public IList<RobotCustomProperty> CustomProperties { get; set; } = [];
}