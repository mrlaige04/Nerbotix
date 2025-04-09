using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Categories;
using Nerbotix.Domain.Robots.Enums;

namespace Nerbotix.Application.Robots.Robots;

public class RobotBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public RobotStatus Status { get; set; }
    public DateTime? LastActivity { get; set; }
    public RobotLocationResponse Location { get; set; } = null!;
    public CategoryBaseResponse Category { get; set; } = null!;
}