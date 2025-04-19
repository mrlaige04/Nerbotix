using Nerbotix.Application.Robots.Robots.CreateRobot;
using Nerbotix.Application.Robots.Robots.UpdateRobot;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Api.Models.Robots;

public class UpdateRobotRequest
{
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
    public IList<UpdateRobotCommandPropertyValue>? UpdatedProperties { get; set; }
    public IList<Guid>? DeletedCustomProperties { get; set; }
    public IList<CreateRobotCommandCustomPropertyItem>? NewCustomProperties { get; set; }
    public IList<UpdateRobotCapabilityItem>? DeletedCapabilities { get; set; }
    public IList<UpdateRobotCapabilityItem>? NewCapabilities { get; set; }
    
    public RobotCommunicationType? CommunicationType { get; set; }
    public CreateRobotHttpCommunication? HttpCommunication { get; set; }
    public CreateRobotMqttCommunication? MqttCommunication { get; set; }
}