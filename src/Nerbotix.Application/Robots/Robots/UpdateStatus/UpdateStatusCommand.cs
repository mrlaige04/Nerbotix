using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Logging;
using Nerbotix.Domain.Tasks;
using NetTopologySuite.Geometries;

namespace Nerbotix.Application.Robots.Robots.UpdateStatus;

public class UpdateStatusCommand : ITenantCommand
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public RobotTaskStatus TaskStatus { get; set; }
    public UpdateStatusCommandPosition? LastPosition { get; set; }
    public IList<UpdateStatusLogCommand>? Logs { get; set; }
}

public class UpdateStatusLogCommand
{
    public LogLevel LogLevel { get; set; }
    public LogScope Scope { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class UpdateStatusCommandPosition
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}