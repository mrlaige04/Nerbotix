using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Infrastructure.Data.Configurations.Tasks;

public class RobotTaskConfiguration : IEntityTypeConfiguration<RobotTask>
{
    public void Configure(EntityTypeBuilder<RobotTask> builder)
    {
        builder.ToTable("robot_tasks");
        builder.HasKey(x => x.Id);
        
        builder.HasOne(t => t.AssignedRobot)
            .WithOne(r => r.CurrentTask)
            .HasForeignKey<RobotTask>(t => t.AssignedRobotId)
            .IsRequired(false);
        
        builder.HasMany(t => t.TaskData)
            .WithOne(t => t.Task)
            .HasForeignKey(t => t.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(t => t.Requirements)
            .WithOne(r => r.Task)
            .HasForeignKey(t => t.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(t => t.Properties)
            .WithOne(p => p.Task)
            .HasForeignKey(p => p.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Archive)
            .WithOne(r => r.Task)
            .HasForeignKey<RobotTaskFiles>(r => r.TaskId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}