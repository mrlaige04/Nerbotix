namespace Nerbotix.Application.Common.Data;

public interface ITenantSeeder
{
    Task SeedRolesAndPermissionsAsync(Guid tenantId);
}