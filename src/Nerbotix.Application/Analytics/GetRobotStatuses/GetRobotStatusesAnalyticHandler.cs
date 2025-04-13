using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Analytics.GetRobotStatuses;

public class GetRobotStatusesAnalyticHandler(
    ITenantRepository<Robot> robotRepository) : IQueryHandler<GetRobotStatusesAnalyticQuery, StatusAnalyticResponse>
{
    public async Task<ErrorOr<StatusAnalyticResponse>> Handle(GetRobotStatusesAnalyticQuery request, CancellationToken cancellationToken)
    {
        var robots = await robotRepository.GetAllWithSelectorAsync(
            r => new { Status = r.Status },
            cancellationToken: cancellationToken);
        
        var statuses = robots
            .GroupBy(r => r.Status)
            .Select(g => new StatusAnalyticResponseItem
            {
                Status = g.Key.ToString(), Count = g.Count()
            });

        return new StatusAnalyticResponse
        {
            Items = statuses.ToList()
        };
    }
}