using RoboTasker.Application.Roles.Permissions;
using RoboTasker.Application.Roles.Roles;

namespace RoboTasker.Application.User;

public class CurrentUserResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string Email { get; set; } = null!;
    public IList<PermissionBaseResponse> Permissions { get; set; } = [];
    public IList<RoleBaseResponse> Roles { get; set; } = [];
}