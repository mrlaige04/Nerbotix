using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Roles;

public class RoleBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
    public bool IsSystem { get; set; }
}