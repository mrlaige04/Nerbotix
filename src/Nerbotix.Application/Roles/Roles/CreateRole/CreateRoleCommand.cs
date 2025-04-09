using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Roles.CreateRole;

public class CreateRoleCommand : ITenantCommand<RoleBaseResponse>
{
    public string Name { get; set; } = null!;

    public IList<Guid> Permissions { get; set; } = [];
}