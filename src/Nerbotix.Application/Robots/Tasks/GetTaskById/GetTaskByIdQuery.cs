using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Tasks.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : ITenantQuery<TaskResponse>;