using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Roles.Roles;

namespace Nerbotix.Application.Users;

public class UserBaseResponse : TenantEntityResponse
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
    public IList<RoleBaseResponse> Roles { get; set; } = [];
}