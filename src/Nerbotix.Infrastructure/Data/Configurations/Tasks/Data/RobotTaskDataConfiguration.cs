using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Infrastructure.Data.Configurations.Tasks.Data;

public class RobotTaskDataConfiguration : IEntityTypeConfiguration<RobotTaskData>
{
    public void Configure(EntityTypeBuilder<RobotTaskData> builder)
    {
        builder.ToTable("task_data");
        builder.HasKey(x => x.Id);
    }
}