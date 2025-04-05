using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Tasks;

public class TaskPropertyResponse : EntityResponse
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}