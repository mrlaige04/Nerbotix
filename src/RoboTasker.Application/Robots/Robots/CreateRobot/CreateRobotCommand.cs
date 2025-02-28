using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Robots.CreateRobot;

public class CreateRobotCommand : ICommand<RobotBaseResponse>
{
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }

    public IList<CreateRobotCommandPropertyItem> Properties { get; set; } = null!;
    public IList<CreateRobotCommandCustomPropertyItem>? CustomProperties { get; set; }
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