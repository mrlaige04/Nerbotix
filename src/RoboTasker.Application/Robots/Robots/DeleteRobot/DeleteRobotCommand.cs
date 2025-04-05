using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Robots.DeleteRobot;

public record DeleteRobotCommand(Guid Id) : ITenantCommand;