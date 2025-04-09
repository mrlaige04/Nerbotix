using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Capabilities;

namespace Nerbotix.Infrastructure.Data.Configurations.Capabilities;

public class CapabilityConfiguration : IEntityTypeConfiguration<Capability>
{
    public void Configure(EntityTypeBuilder<Capability> builder)
    {
        builder.ToTable("capabilities");
        builder.HasKey(capability => capability.Id);
        
        builder.HasMany(c => c.Robots)
            .WithOne(robot => robot.Capability)
            .HasForeignKey(robot => robot.CapabilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}