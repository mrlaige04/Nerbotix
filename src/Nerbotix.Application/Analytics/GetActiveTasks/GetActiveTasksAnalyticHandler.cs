using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Analytics.GetActiveTasks;

public class GetActiveTasksAnalyticHandler(
    ITenantRepository<RobotTask> taskRepository) : IQueryHandler<GetActiveTasksAnalyticQuery, PaginatedList<GetActiveTasksAnalyticResponseItem>>
{
    public async Task<ErrorOr<PaginatedList<GetActiveTasksAnalyticResponseItem>>> Handle(GetActiveTasksAnalyticQuery request, CancellationToken cancellationToken)
    {
         return await taskRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            t => new GetActiveTasksAnalyticResponseItem
            {
                Id = t.Id,
                Name = t.Name,
                Priority = t.Priority,
            },
            t => t.Status == RobotTaskStatus.Pending,
            q => q.OrderBy(t => t.Priority),
            cancellationToken: cancellationToken);
    }
}