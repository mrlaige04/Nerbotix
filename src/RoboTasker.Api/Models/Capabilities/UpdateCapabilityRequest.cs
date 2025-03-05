using RoboTasker.Application.Robots.Capabilities.CreateCapability;

namespace RoboTasker.Api.Models.Capabilities;

public class UpdateCapabilityRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IList<Guid>? DeletedCapabilities { get; set; }
    public IList<CreateCapabilityItem>? NewCapabilities { get; set; }
}