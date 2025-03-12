using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Infrastructure.Data.Configurations.Robots;

public class RobotPropertyValueConfiguration : IEntityTypeConfiguration<RobotPropertyValue>
{
    public void Configure(EntityTypeBuilder<RobotPropertyValue> builder)
    {
        builder.ToTable("robot_properties_values");
        builder.HasKey(p => new { p.RobotId, p.PropertyId });
        
        builder.Property(p => p.Value)
            .IsRequired();
    }
}