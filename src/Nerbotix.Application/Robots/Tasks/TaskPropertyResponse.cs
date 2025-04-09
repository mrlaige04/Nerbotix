using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Tasks;

public class TaskPropertyResponse : EntityResponse
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}