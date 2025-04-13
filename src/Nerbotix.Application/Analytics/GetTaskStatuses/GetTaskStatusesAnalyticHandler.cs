using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Analytics.GetTaskStatuses;

public class GetTaskStatusesAnalyticHandler(
    ITenantRepository<RobotTask> tasksRepository) : IQueryHandler<GetTaskStatusesAnalyticQuery, StatusAnalyticResponse>
{
    public async Task<ErrorOr<StatusAnalyticResponse>> Handle(GetTaskStatusesAnalyticQuery request, CancellationToken cancellationToken)
    {
        var tasks = await tasksRepository.GetAllWithSelectorAsync(
            t => new { Status = t.Status.ToString() },
            cancellationToken: cancellationToken);
        
        var statuses = tasks
            .GroupBy(t => t.Status)
            .Select(g => new StatusAnalyticResponseItem
            {
                Status = g.Key, Count = g.Count()
            });

        return new StatusAnalyticResponse
        {
            Items = statuses.ToList()
        };
    }
}