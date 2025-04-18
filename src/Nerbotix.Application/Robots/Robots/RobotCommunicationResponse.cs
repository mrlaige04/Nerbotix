using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots;

public class RobotCommunicationResponse
{
    public RobotCommunicationType CommunicationType { get; set; }
    
    public string? ApiEndpoint { get; set; }
    public string? HttpMethod { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    
    public string? MqttBrokerAddress { get; set; }
    public string? MqttBrokerUsername { get; set; }
    public string? MqttBrokerPassword { get; set; }
    public string? MqttTopic { get; set; }
}