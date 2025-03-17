using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Infrastructure.Data.Configurations.Robots;

public class RobotConfiguration : IEntityTypeConfiguration<Robot>
{
    public void Configure(EntityTypeBuilder<Robot> builder)
    {
        builder.ToTable("robots");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.Name }).IsUnique();
        
        builder.HasOne(r => r.Communication)
            .WithOne(c => c.Robot)
            .HasForeignKey<RobotCommunication>(c => c.RobotId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.Properties)
            .WithOne(p => p.Robot)
            .HasForeignKey(p => p.RobotId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.CustomProperties)
            .WithOne(p => p.Robot)
            .HasForeignKey(p => p.RobotId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(r => r.Location)
            .WithOne(l => l.Robot)
            .HasForeignKey<RobotLocation>(l => l.RobotId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.Capabilities)
            .WithOne(p => p.Robot)
            .HasForeignKey(p => p.RobotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}