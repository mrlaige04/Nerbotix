using Nerbotix.Domain.Logging;

namespace Nerbotix.Application.Robots;

public class LogResponse
{
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}