using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Infrastructure.Data.Configurations.Tasks.Data;

public class RobotTaskDataConfiguration : IEntityTypeConfiguration<RobotTaskData>
{
    public void Configure(EntityTypeBuilder<RobotTaskData> builder)
    {
        builder.ToTable("task_data");
        builder.HasKey(x => x.Id);
    }
}