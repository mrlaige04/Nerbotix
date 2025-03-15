using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Roles.CreateRole;

public class CreateRoleCommand : ITenantCommand<RoleBaseResponse>
{
    public string Name { get; set; } = null!;

    public IList<Guid> Permissions { get; set; } = [];
}