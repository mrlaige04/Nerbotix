namespace Nerbotix.Domain.Robots.Communications;

public class MqttCommunication : RobotCommunication
{
    public string MqttBrokerAddress { get; set; } = null!;
    public string MqttBrokerUsername { get; set; } = null!;
    public string MqttBrokerPassword { get; set; } = null!;
    public string MqttTopic { get; set; } = null!;
}