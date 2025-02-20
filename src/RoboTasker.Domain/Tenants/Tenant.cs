using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;
}