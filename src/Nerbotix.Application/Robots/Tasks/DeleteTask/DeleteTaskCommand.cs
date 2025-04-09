using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Tasks.DeleteTask;

public record DeleteTaskCommand(Guid Id) : ITenantCommand;