using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Robots.CreateRobot;

namespace Nerbotix.Application.Robots.Robots.UpdateRobot;

public class UpdateRobotCommand : ITenantCommand<RobotBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
    public IList<UpdateRobotCommandPropertyValue>? UpdatedProperties { get; set; }
    public IList<Guid>? DeletedCustomProperties { get; set; }
    public IList<CreateRobotCommandCustomPropertyItem>? NewCustomProperties { get; set; }
    public IList<UpdateRobotCapabilityItem>? DeletedCapabilities { get; set; }
    public IList<UpdateRobotCapabilityItem>? NewCapabilities { get; set; }
}

public class UpdateRobotCommandPropertyValue
{
    public Guid PropertyId { get; set; }
    public object Value { get; set; } = null!;
}

public class UpdateRobotCapabilityItem
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
}