using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots;

public class RobotLocationResponse : EntityResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}