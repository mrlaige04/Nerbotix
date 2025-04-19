namespace Nerbotix.TestRobotApi.Models;

public class UpdateStatus
{
    public Guid TaskId { get; set; }
    public RobotTaskStatus TaskStatus { get; set; }
    public Position LastPosition { get; set; } = null!;
    public IList<Log> Logs { get; set; } = [];
}

public class Position
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public enum RobotTaskStatus
{
    Pending,
    WaitingForEnqueue,
    InProgress,
    Completed,
    Failed,
    Canceled
}

public enum LogScope
{
    Robot,
    Task
}

public enum RoboLogLevel
{
    Info,
    Warning,
    Error
}

public class Log
{
    public RoboLogLevel LogLevel { get; set; }
    public LogScope Scope { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Timestamp { get; set; }
}