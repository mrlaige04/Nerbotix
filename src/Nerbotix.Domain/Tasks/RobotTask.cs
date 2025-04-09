using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Domain.Tasks;

public class RobotTask : TenantEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    

    public DateTime? StartedAt { get; set; }
    public RobotTaskStatus Status { get; set; } = RobotTaskStatus.Pending;
    public DateTimeOffset? CompletedAt { get; set; }
    
    public Robot? AssignedRobot { get; set; }
    public Guid? AssignedRobotId { get; set; }
    
    public DateTime AssignedAt { get; set; }
    
    public TimeSpan? EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }

    public RobotCategory Category { get; set; } = null!;
    public Guid CategoryId { get; set; }

    public IList<RobotTaskData> TaskData { get; set; } = [];
    public IList<RobotTaskRequirement> Requirements { get; set; } = [];
    public RobotTaskFiles? Archive { get; set; }
}