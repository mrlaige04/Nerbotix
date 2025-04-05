using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Tasks.DeleteTask;

public record DeleteTaskCommand(Guid Id) : ITenantCommand;