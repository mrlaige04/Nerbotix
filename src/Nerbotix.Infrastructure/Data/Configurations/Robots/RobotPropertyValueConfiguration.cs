using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Infrastructure.Data.Configurations.Robots;

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