namespace Nerbotix.Application.Robots.Robots;

public class RobotResponse : RobotBaseResponse
{
    public IList<RobotPropertyResponse> Properties { get; set; } = [];
    public IList<RobotCustomPropertyResponse>? CustomProperties { get; set; }
    public IList<RobotCapabilityResponse> Capabilities { get; set; } = [];
    public IList<LogResponse> Logs { get; set; } = [];
    public RobotCommunicationResponse Communication { get; set; } = null!;
}