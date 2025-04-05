using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Tasks.GetTasks;

public class GetTasksQuery : ITenantQuery<PaginatedList<TaskBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}