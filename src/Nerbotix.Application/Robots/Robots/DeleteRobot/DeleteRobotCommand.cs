using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Robots.DeleteRobot;

public record DeleteRobotCommand(Guid Id) : ITenantCommand;