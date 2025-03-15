using RoboTasker.Application.Roles.Permissions;

namespace RoboTasker.Infrastructure.Authentication;

public class CurrentUserResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = null!;
    public IList<PermissionBaseResponse> Permissions { get; set; } = [];
}