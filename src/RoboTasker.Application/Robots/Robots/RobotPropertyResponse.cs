using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Robots;

public class RobotPropertyResponse : TenantEntityResponse
{
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public RobotPropertyType Type { get; set; } 
}