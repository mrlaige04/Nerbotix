using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tasks.Data;

public class RobotTaskData : TenantEntity
{
    public string Key { get; set; } = null!;
    public RobotTaskDataType Type { get; set; }
    
    public string Value { get; set; } = null!;
    
    public RobotTask Task { get; set; } = null!;
    public Guid TaskId { get; set; }
}