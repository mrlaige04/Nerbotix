using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Infrastructure.Data.Configurations.Tasks;

public class RobotTaskConfiguration : IEntityTypeConfiguration<RobotTask>
{
    public void Configure(EntityTypeBuilder<RobotTask> builder)
    {
        builder.ToTable("robot_tasks");
        builder.HasKey(x => x.Id);
        
        builder.HasMany(t => t.TaskData)
            .WithOne(t => t.Task)
            .HasForeignKey(t => t.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(t => t.Requirements)
            .WithOne(r => r.Task)
            .HasForeignKey(t => t.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Archive)
            .WithOne(r => r.Task)
            .HasForeignKey<RobotTaskFiles>(r => r.TaskId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasMany(t => t.Logs)
            .WithOne(l => l.Task)
            .HasForeignKey(l => l.TaskId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}