using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.GetCapabilityById;

public record GetCapabilityByIdQuery(Guid Id) : ITenantQuery<CapabilityResponse>;