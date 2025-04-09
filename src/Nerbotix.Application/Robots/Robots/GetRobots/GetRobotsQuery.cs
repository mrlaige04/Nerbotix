using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Robots.Robots.GetRobots;

public class GetRobotsQuery : ITenantQuery<PaginatedList<RobotBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}