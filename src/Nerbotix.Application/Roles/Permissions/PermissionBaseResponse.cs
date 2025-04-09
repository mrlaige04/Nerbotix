using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Permissions;

public class PermissionBaseResponse : TenantEntityResponse
{
    public string GroupName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
}