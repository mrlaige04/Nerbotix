using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Infrastructure.Data.Configurations.Tasks.Data;

public class RobotTaskFilesConfiguration : IEntityTypeConfiguration<RobotTaskFiles>
{
    public void Configure(EntityTypeBuilder<RobotTaskFiles> builder)
    {
        builder.ToTable("task_archives");
        builder.HasKey(x => x.Id);
    }
}