using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Tenants.Settings.Algorithms;

namespace Nerbotix.Domain.Tenants.Settings;

public class TenantSettings : TenantEntity
{
    public TenantAlgorithmSettings AlgorithmSettings { get; set; } = null!;

    public static TenantSettings CreateDefault(Guid tenantId)
    {
        return new TenantSettings
        {
            TenantId = tenantId,
            AlgorithmSettings = TenantAlgorithmSettings.CreateDefault(tenantId)
        };
    }
}