using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Robots.Capabilities.CreateCapability;

namespace RoboTasker.Application.Robots.Capabilities.UpdateCapability;

public class UpdateCapabilityCommand : ICommand<CapabilityBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IList<Guid>? DeletedCapabilities { get; set; }
    public IList<CreateCapabilityItem>? NewCapabilities { get; set; }
}