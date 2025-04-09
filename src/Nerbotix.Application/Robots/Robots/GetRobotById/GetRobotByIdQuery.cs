using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Robots.GetRobotById;

public record GetRobotByIdQuery(Guid Id) : ITenantQuery<RobotResponse>;