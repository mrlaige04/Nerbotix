using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Robots;

public class RobotProperty : TenantEntity
{
    public string Name { get; set; } = null!;
    
    public RobotPropertyType Type { get; set; }

    public string? Unit { get; set; } = string.Empty;
    
    public IList<RobotPropertyValue> Values { get; set; } = [];

    public double Factor { get; set; } = 1.0;

    public RobotCategory Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}