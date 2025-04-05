using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Robots;

public class RobotCustomPropertyResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}