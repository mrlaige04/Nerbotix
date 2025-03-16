using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Roles.Roles;

public class RoleBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
}