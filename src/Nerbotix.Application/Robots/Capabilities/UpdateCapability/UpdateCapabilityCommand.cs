using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Capabilities.CreateCapability;

namespace Nerbotix.Application.Robots.Capabilities.UpdateCapability;

public class UpdateCapabilityCommand : ITenantCommand<CapabilityBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IList<Guid>? DeletedCapabilities { get; set; }
    public IList<CreateCapabilityItem>? NewCapabilities { get; set; }
}