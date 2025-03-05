using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Robots.Categories;
using RoboTasker.Domain.Robots.Enums;

namespace RoboTasker.Application.Robots.Robots;

public class RobotBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public RobotStatus Status { get; set; }
    public DateTime? LastActivity { get; set; }
    public RobotLocationResponse Location { get; set; } = null!;
    public CategoryBaseResponse Category { get; set; } = null!;
}