using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.SuperAdmin.Tenants.CreateTenant;

public class CreateTenantCommand : ISuperAdminCommand<TenantBaseResponse>
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
}