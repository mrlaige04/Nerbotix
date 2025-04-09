using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Infrastructure.Data.Configurations.Tasks;

public class RobotTaskRequirementConfiguration : IEntityTypeConfiguration<RobotTaskRequirement>
{
    public void Configure(EntityTypeBuilder<RobotTaskRequirement> builder)
    {
        builder.ToTable("task_requirements");
        builder.HasKey(x => new { x.TaskId, x.CapabilityId });
    }
}