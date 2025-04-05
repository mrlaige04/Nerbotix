using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public class RobotPropertyValue : TenantEntity
{
    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }
    
    public RobotProperty Property { get; set; } = null!;
    public Guid PropertyId { get; set; }
    
    public string Value { get; set; } = null!;
}