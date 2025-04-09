using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots;

public class RobotLocationResponse : EntityResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}