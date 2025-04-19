using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots.CreateRobot;

public class CreateRobotCommand : ITenantCommand<RobotBaseResponse>
{
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }

    public IList<CreateRobotCommandPropertyItem> Properties { get; set; } = null!;
    public IList<CreateRobotCommandCustomPropertyItem>? CustomProperties { get; set; }
    public IList<CreateRobotCommandCapability>? Capabilities { get; set; }
    
    public RobotCommunicationType CommunicationType { get; set; }
    public CreateRobotHttpCommunication? HttpCommunication { get; set; }
    public CreateRobotMqttCommunication? MqttCommunication { get; set; }
}

public class CreateRobotCommandPropertyItem
{
    public Guid PropertyId { get; set; }
    public object Value { get; set; } = null!;
}

public class CreateRobotCommandCustomPropertyItem
{
    public string Name { get; set; } = null!;
    public object Value { get; set; } = null!;
}

public class CreateRobotCommandCapability
{
    public Guid GroupId { get; set; }
    public Guid Id { get; set; }
}

public class CreateRobotHttpCommunication
{
    public string Url { get; set; } = null!;
    public string Method { get; set; } = null!;
    public IList<CreateCommunicationHttpHeader>? Headers { get; set; }
}

public class CreateCommunicationHttpHeader
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
}

public class CreateRobotMqttCommunication
{
    public string MqttBrokerAddress { get; set; } = null!;
    public string MqttBrokerUsername { get; set; } = null!;
    public string MqttBrokerPassword { get; set; } = null!;
    public string MqttTopic { get; set; } = null!;
}