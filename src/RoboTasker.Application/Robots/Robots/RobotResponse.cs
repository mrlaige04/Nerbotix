namespace RoboTasker.Application.Robots.Robots;

public class RobotResponse : RobotBaseResponse
{
    public IList<RobotPropertyResponse> Properties { get; set; } = [];
    public IList<RobotCustomPropertyResponse>? CustomProperties { get; set; }
}