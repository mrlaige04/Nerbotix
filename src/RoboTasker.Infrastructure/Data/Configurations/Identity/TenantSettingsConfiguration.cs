using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Infrastructure.Data.Configurations.Identity;

public class TenantSettingsConfiguration : IEntityTypeConfiguration<TenantSettings>
{
    public void Configure(EntityTypeBuilder<TenantSettings> builder)
    {
        builder.ToTable("tenant_settings");
        builder.HasKey(x => x.Id);
        
        builder.OwnsOne(s => s.LoadBalancingAlgorithmSettings);
        builder.OwnsOne(s => s.GeneticAlgorithmSettings);
        builder.OwnsOne(s => s.AntColonyAlgorithmSettings);
        builder.OwnsOne(s => s.SimulatedAnnealingAlgorithmSettings);
    }
}