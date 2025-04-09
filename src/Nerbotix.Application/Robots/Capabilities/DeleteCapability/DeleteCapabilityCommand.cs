using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.DeleteCapability;

public record DeleteCapabilityCommand(Guid Id) : ITenantCommand;