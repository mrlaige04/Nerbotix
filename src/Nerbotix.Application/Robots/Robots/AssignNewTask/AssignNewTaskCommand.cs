using Nerbotix.Application.Robots.Tasks;

namespace Nerbotix.Application.Robots.Robots.AssignNewTask;

public class AssignNewTaskCommand
{
    public Guid TaskId { get; set; }
    public Guid RobotId { get; set; }
    public string RobotName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }
    public IList<TaskDataResponse> Data { get; set; } = [];
}