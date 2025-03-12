using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Infrastructure.Data.Configurations.Tasks.Data;

public class RobotTaskFilesConfiguration : IEntityTypeConfiguration<RobotTaskFiles>
{
    public void Configure(EntityTypeBuilder<RobotTaskFiles> builder)
    {
        builder.ToTable("task_archives");
        builder.HasKey(x => x.Id);
    }
}