using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Infrastructure.Data.Configurations.Tasks;

public class RobotTaskRequirementConfiguration : IEntityTypeConfiguration<RobotTaskRequirement>
{
    public void Configure(EntityTypeBuilder<RobotTaskRequirement> builder)
    {
        builder.ToTable("task_requirements");
        builder.HasKey(x => new { x.TaskId, x.CapabilityId });
    }
}