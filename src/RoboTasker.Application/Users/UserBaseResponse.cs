using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Users;

public class UserBaseResponse : TenantEntityResponse
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool EmailVerified { get; set; }
}