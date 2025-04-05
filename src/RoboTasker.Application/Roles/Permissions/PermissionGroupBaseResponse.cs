using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Permissions;

public class PermissionGroupBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
}