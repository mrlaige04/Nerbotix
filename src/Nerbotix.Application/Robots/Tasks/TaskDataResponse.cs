using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Application.Robots.Tasks;

public class TaskDataResponse : EntityResponse
{
    public string Key { get; set; } = null!;
    public RobotTaskDataType Type { get; set; }
    
    public string Value { get; set; } = string.Empty;
}