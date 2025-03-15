using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Roles.UpdateRole;

public class UpdateRoleCommand : ITenantCommand<RoleBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<Guid>? Permissions { get; set; }
}