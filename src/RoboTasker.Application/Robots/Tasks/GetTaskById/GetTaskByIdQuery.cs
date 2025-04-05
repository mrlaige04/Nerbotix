using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Tasks.GetTaskById;

public record GetTaskByIdQuery(Guid Id) : ITenantQuery<TaskResponse>;