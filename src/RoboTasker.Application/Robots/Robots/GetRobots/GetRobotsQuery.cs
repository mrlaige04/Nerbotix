using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Robots.GetRobots;

public class GetRobotsQuery : IQuery<PaginatedList<RobotBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}