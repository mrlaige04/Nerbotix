using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Robots.Categories;

namespace RoboTasker.Application.Robots.Robots;

public class RobotBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public CategoryBaseResponse Category { get; set; } = null!;
}