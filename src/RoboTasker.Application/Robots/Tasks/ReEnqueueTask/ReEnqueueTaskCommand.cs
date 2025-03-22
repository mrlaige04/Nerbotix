using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Tasks.ReEnqueueTask;

public record ReEnqueueTaskCommand(Guid Id) : ITenantCommand;