using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.SuperAdmin.Tenants.GetTenants;

public class GetTenantsHandler(
    IBaseRepository<Tenant> tenantRepository,
    IConfiguration configuration) : IQueryHandler<GetTenantsQuery, PaginatedList<TenantBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<TenantBaseResponse>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var superAdminEmail = configuration["SuperAdmin:Email"];
        var tenants = await tenantRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            t => new TenantBaseResponse
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
            },
            t => !EF.Functions.Like(t.Email, $"%{superAdminEmail}%"),
            cancellationToken: cancellationToken);

        return tenants;
    }
}