using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Categories;

public class CategoryPropertyResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public double Factor  { get; set; }
    public string? Unit { get; set; }
    public RobotPropertyType Type { get; set; }
}