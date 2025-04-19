namespace Nerbotix.TestRobotApi.Models;

public class StartTask
{
    public Guid TaskId { get; set; }
    public Guid RobotId { get; set; }
    public string RobotName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }
    public IList<TaskData> Data { get; set; } = [];
}

public class TaskData
{
    public string Key { get; set; } = null!;
    public TaskDataType Type { get; set; }
    
    public string Value { get; set; } = string.Empty;
}

public enum TaskDataType
{
    String,
    Number,
    Boolean,
    Json,
    DateTime
}