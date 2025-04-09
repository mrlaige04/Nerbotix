using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots;

public class RobotPropertyResponse : TenantEntityResponse
{
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public RobotPropertyType Type { get; set; } 
}