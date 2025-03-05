using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilityById;

public record GetCapabilityByIdQuery(Guid Id) : IQuery<CapabilityResponse>;