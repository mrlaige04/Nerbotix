using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Tasks;

public class TaskFileResponse : EntityResponse
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; }
}