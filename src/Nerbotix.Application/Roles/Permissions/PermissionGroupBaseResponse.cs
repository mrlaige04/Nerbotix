using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Permissions;

public class PermissionGroupBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
}