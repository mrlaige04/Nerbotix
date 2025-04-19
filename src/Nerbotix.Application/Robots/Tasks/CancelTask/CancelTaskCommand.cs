using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Tasks.CancelTask;

public record CancelTaskCommand(Guid Id): ITenantCommand;