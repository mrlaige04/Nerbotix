using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Robots.CreateRobot;

public class CreateRobotCommand : ITenantCommand<RobotBaseResponse>
{
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }

    public IList<CreateRobotCommandPropertyItem> Properties { get; set; } = null!;
    public IList<CreateRobotCommandCustomPropertyItem>? CustomProperties { get; set; }
    public IList<CreateRobotCommandCapability>? Capabilities { get; set; }
}

public class CreateRobotCommandPropertyItem
{
    public Guid PropertyId { get; set; }
    public object Value { get; set; } = null!;
}

public class CreateRobotCommandCustomPropertyItem
{
    public string Name { get; set; } = null!;
    public object Value { get; set; } = null!;
}

public class CreateRobotCommandCapability
{
    public Guid GroupId { get; set; }
    public Guid Id { get; set; }
}