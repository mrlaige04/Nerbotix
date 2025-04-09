using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Tasks.ReEnqueueTask;

public record ReEnqueueTaskCommand(Guid Id) : ITenantCommand;