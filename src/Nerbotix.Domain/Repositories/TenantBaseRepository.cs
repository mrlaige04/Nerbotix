using Microsoft.EntityFrameworkCore;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Domain.Repositories;

public class TenantBaseRepository<TTenantEntity>(
    DbContext dbContext,
    ICurrentUser currentUser) 
    : BaseRepository<TTenantEntity>(dbContext), ITenantRepository<TTenantEntity>
    where TTenantEntity : class, ITenantEntity<Guid>
{
    public override async Task<IQueryable<TTenantEntity>> GetQuery(CancellationToken cancellationToken = default)
    {
        var tenantId = currentUser.GetTenantId();
        var baseQuery = await base.GetQuery(cancellationToken);
        
        return tenantId.HasValue ? baseQuery.Where(t => t.TenantId == tenantId) : baseQuery;
    }
}