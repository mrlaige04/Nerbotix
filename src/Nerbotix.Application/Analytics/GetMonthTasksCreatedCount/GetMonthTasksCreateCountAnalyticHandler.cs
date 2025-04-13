using ErrorOr;
using Nerbotix.Application.Analytics.GetRobotStatuses;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Analytics.GetMonthTasksCreatedCount;

public class GetMonthTasksCreateCountAnalyticHandler(
    ITenantRepository<RobotTask> tasksRepository) : IQueryHandler<GetMonthTasksCreateCountAnalyticQuery, GetMonthTasksCreateCountAnalyticResponse>
{
    public async Task<ErrorOr<GetMonthTasksCreateCountAnalyticResponse>> Handle(GetMonthTasksCreateCountAnalyticQuery request, CancellationToken cancellationToken)
    {
        var tasks = await tasksRepository.GetAllWithSelectorAsync(
            t => t.CreatedAt,
            cancellationToken: cancellationToken);

        var days = tasks
            .GroupBy(d => d.Day)
            .OrderBy(d => d.Key)
            .Select(g => new GetMonthTasksCreateCountAnalyticResponseItem
            {
                Day = g.Key,
                Count = g.Count()
            }).ToList();

        var now = DateTime.UtcNow;
        var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

        var result = Enumerable.Range(1, daysInMonth)
            .Select(d =>
            {
                var day = days.FirstOrDefault(x => x.Day == d);
                return new GetMonthTasksCreateCountAnalyticResponseItem
                {
                    Day = d,
                    Count = day?.Count ?? 0,
                };
            });
        
        return new GetMonthTasksCreateCountAnalyticResponse
        {
            Items = result.ToList()
        };
    }
}