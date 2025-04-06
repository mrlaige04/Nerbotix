using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Tenants.Settings.Algorithms;

namespace RoboTasker.Domain.Tenants.Settings;

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