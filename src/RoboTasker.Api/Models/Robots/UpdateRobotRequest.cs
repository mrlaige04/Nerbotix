using RoboTasker.Application.Robots.Robots.CreateRobot;
using RoboTasker.Application.Robots.Robots.UpdateRobot;

namespace RoboTasker.Api.Models.Robots;

public class UpdateRobotRequest
{
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
    public IList<UpdateRobotCommandPropertyValue>? UpdatedProperties { get; set; }
    public IList<Guid>? DeletedCustomProperties { get; set; }
    public IList<CreateRobotCommandCustomPropertyItem>? NewCustomProperties { get; set; }
    public IList<UpdateRobotCapabilityItem>? DeletedCapabilities { get; set; }
    public IList<UpdateRobotCapabilityItem>? NewCapabilities { get; set; }
}