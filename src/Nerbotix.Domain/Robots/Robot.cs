using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Robots.Enums;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Domain.Robots;

public class Robot : TenantEntity
{
    public string Name { get; set; } = null!;
    
    public RobotCategory Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
    
    public RobotStatus Status { get; set; }
    public DateTime? LastActivity { get; set; }

    public RobotLocation Location { get; set; } = null!;
    public Guid LocationId { get; set; }
    
    public RobotCommunication Communication { get; set; } = null!;

    public IList<RobotTask> TasksQueue { get; set; } = [];
    
    public IList<RobotPropertyValue> Properties { get; set; } = [];
    public IList<RobotCustomProperty> CustomProperties { get; set; } = [];
    public IList<RobotCapability> Capabilities { get; set; } = [];
}