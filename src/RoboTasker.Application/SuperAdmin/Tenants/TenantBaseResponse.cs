using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.SuperAdmin.Tenants;

public class TenantBaseResponse : EntityResponse
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}