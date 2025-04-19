using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Robots.GetNextTask;

public record GetNextTaskCommand(Guid Id) : ITenantCommand<GetNextTaskCommandResponse>;

public record GetNextTaskCommandResponse(Guid? TaskId);