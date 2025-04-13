using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Analytics.GetActiveTasks;

public class GetActiveTasksAnalyticQuery : ITenantQuery<PaginatedList<GetActiveTasksAnalyticResponseItem>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}