using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tasks;

public class RobotTaskProperty : TenantEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    
    public RobotTask Task { get; set; } = null!;
    public Guid TaskId { get; set; }
}