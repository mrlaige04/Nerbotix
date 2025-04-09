using Nerbotix.Application.Roles.Permissions;

namespace Nerbotix.Application.Roles.Roles;

public class RoleResponse : RoleBaseResponse
{
    public IList<PermissionBaseResponse>? Permissions { get; set; }
}