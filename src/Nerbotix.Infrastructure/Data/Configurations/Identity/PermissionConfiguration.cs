using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Data.Configurations.Identity;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name).IsRequired();
        builder.HasIndex(p => new { p.Name, p.GroupId }).IsUnique();
        
        builder.HasMany(p => p.Roles)
            .WithOne(p => p.Permission)
            .HasForeignKey(p => p.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}