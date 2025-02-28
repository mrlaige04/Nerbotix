using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public class RobotProperty : TenantEntity
{
    public string Name { get; set; } = null!;
    
    public RobotPropertyType Type { get; set; }

    public IList<RobotPropertyValue> Values { get; set; } = [];

    public RobotCategory Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}