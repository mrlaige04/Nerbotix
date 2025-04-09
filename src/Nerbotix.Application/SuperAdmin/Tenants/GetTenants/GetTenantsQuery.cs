using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.SuperAdmin.Tenants.GetTenants;

public class GetTenantsQuery : IQuery<PaginatedList<TenantBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}