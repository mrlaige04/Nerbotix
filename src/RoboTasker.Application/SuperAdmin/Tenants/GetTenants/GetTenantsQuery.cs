using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.SuperAdmin.Tenants.GetTenants;

public class GetTenantsQuery : IQuery<PaginatedList<TenantBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}