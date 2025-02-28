using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Robots.GetRobotById;

public record GetRobotByIdQuery(Guid Id) : IQuery<RobotResponse>;