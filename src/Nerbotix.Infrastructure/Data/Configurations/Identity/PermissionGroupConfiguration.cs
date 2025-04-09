using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Data.Configurations.Identity;

public class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
    public void Configure(EntityTypeBuilder<PermissionGroup> builder)
    {
        builder.ToTable("permission_groups");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name).IsRequired();
        builder.HasIndex(p => new { p.TenantId, p.Name }).IsUnique();
        
        builder.HasMany(p => p.Permissions)
            .WithOne(p => p.Group)
            .HasForeignKey(p => p.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}