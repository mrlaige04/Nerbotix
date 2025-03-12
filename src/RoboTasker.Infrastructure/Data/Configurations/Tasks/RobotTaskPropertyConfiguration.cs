using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Data.Configurations.Tasks;

public class RobotTaskPropertyConfiguration : IEntityTypeConfiguration<RobotTaskProperty>
{
    public void Configure(EntityTypeBuilder<RobotTaskProperty> builder)
    {
        builder.ToTable("task_properties");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Value).IsRequired();
    }
}