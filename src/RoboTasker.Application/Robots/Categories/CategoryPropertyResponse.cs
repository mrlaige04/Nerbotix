using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Categories;

public class CategoryPropertyResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    
    public RobotPropertyType Type { get; set; }
}