using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Data.Configurations.Identity;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Name).IsUnique();
        
        builder
            .HasMany(x => x.Users)
            .WithOne(x => x.Tenant)
            .HasForeignKey(x => x.TenantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(x => x.Roles)
            .WithOne(x => x.Tenant)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}