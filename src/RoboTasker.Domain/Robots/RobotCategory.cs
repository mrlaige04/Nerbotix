using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public class RobotCategory : TenantEntity
{
    public string Name { get; set; } = null!;
    
    public IList<RobotProperty> Properties { get; set; } = [];
    public IList<Robot> Robots { get; set; } = [];
}