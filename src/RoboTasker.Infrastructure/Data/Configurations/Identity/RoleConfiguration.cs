using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Data.Configurations.Identity;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(x => x.Id);

        builder
            .HasMany(r => r.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);
        
        builder.HasMany(r => r.Permissions)
            .WithOne(p => p.Role)
            .HasForeignKey(r => r.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}