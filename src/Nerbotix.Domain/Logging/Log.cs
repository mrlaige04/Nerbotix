using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Domain.Logging;

public class Log : TenantEntity
{
    public LogLevel Level { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset Timestamp { get; set; }
    
    public LogScope Scope { get; set; }
    
    public Robot? Robot { get; set; }
    public Guid? RobotId { get; set; }
    
    public RobotTask? Task { get; set; }
    public Guid? TaskId { get; set; }
}