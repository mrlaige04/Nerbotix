using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Infrastructure.Data.Configurations.Identity.Settings;

public class TenantSettingsConfiguration : IEntityTypeConfiguration<TenantSettings>
{
    public void Configure(EntityTypeBuilder<TenantSettings> builder)
    {
        builder.ToTable("tenant_settings");
        builder.HasKey(x => x.Id);
        
        builder.OwnsOne(s => s.AlgorithmSettings, a =>
        {
            a.OwnsOne(set => set.LoadBalancingAlgorithmSettings);
            a.OwnsOne(set => set.GeneticAlgorithmSettings);
            a.OwnsOne(set => set.AntColonyAlgorithmSettings);
            a.OwnsOne(set => set.SimulatedAnnealingAlgorithmSettings);
        });
    }
}