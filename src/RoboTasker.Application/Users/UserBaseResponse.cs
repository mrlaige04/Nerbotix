using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Roles.Roles;

namespace RoboTasker.Application.Users;

public class UserBaseResponse : TenantEntityResponse
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public IList<RoleBaseResponse> Roles { get; set; } = [];
}