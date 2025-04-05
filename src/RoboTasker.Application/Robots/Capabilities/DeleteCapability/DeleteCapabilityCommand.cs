using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.DeleteCapability;

public record DeleteCapabilityCommand(Guid Id) : ITenantCommand;