using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;

namespace RoboTasker.Domain.Repositories;

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