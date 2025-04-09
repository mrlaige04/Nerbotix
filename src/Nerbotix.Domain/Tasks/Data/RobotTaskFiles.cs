using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Domain.Tasks.Data;

public class RobotTaskFiles : TenantEntity
{
    public string Url { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = "application/zip";
    public long Size { get; set; }
    
    public RobotTask? Task { get; set; }
    public Guid? TaskId { get; set; }
}