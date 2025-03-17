using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Tenants;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IList<User> Users { get; set; } = [];
    public IList<Role> Roles { get; set; } = [];
    public IList<PermissionGroup> PermissionGroups { get; set; } = [];
}