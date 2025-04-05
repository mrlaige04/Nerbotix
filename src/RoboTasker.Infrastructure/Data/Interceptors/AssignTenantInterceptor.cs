using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Services;

namespace RoboTasker.Infrastructure.Data.Interceptors;

public class AssignTenantInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        
        var tenantId = currentUser.GetTenantId();
        if (!tenantId.HasValue) return;
        
        foreach (var entry in context!.ChangeTracker.Entries<ITenantEntity<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = tenantId.Value;
            }
        }
    }
}