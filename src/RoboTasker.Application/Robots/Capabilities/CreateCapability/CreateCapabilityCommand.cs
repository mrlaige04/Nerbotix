using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.CreateCapability;

public class CreateCapabilityCommand : ITenantCommand<CapabilityBaseResponse>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public IList<CreateCapabilityItem> Capabilities { get; set; } = [];
}

public class CreateCapabilityItem
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}