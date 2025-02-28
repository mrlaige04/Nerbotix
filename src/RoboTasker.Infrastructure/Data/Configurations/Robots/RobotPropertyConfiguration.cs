using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Infrastructure.Data.Configurations.Robots;

public class RobotPropertyConfiguration : IEntityTypeConfiguration<RobotProperty>
{
    public void Configure(EntityTypeBuilder<RobotProperty> builder)
    {
        builder.ToTable("robot_category_properties");
        builder.HasKey(x => x.Id);
        
        builder.HasMany(p => p.Values)
            .WithOne(p => p.Property)
            .HasForeignKey(p => p.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}