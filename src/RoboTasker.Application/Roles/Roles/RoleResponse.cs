using RoboTasker.Application.Roles.Permissions;

namespace RoboTasker.Application.Roles.Roles;

public class RoleResponse : RoleBaseResponse
{
    public IList<PermissionBaseResponse>? Permissions { get; set; }
}