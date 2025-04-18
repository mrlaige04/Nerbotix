using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Robots.Communications;

namespace Nerbotix.Infrastructure.Data.Configurations.Robots;

public class RobotCommunicationConfiguration : IEntityTypeConfiguration<RobotCommunication>
{
    public void Configure(EntityTypeBuilder<RobotCommunication> builder)
    {
        builder.ToTable("robot_communications");
        builder.HasKey(c => c.Id);
        
        builder.HasDiscriminator<RobotCommunicationType>(nameof(RobotCommunication.Type))
            .HasValue<MqttCommunication>(RobotCommunicationType.Mqtt)
            .HasValue<HttpCommunication>(RobotCommunicationType.Http);
    }
}