using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Infrastructure.Data.Configurations.Robots;

public class RobotCustomPropertyConfiguration : IEntityTypeConfiguration<RobotCustomProperty>
{
    public void Configure(EntityTypeBuilder<RobotCustomProperty> builder)
    {
        builder.ToTable("robot_custom_properties");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => new { x.RobotId, x.Name }).IsUnique();
        
        builder.Property(x => x.Value)
            .IsRequired();
    }
}