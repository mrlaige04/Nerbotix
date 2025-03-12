using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Application.Robots.Tasks;

public class TaskDataResponse : EntityResponse
{
    public string Key { get; set; } = null!;
    public RobotTaskDataType Type { get; set; }
    
    public string Value { get; set; } = string.Empty;
}