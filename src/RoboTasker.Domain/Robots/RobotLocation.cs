using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Domain.Robots;

public class RobotLocation : TenantEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
    public Robot Robot { get; set; } = null!;
    public Guid RobotId { get; set; }

    public static RobotLocation Create(double latitude, double longitude) =>
        new()
        {
            Latitude = latitude,
            Longitude = longitude,
            Timestamp = DateTimeOffset.Now.UtcDateTime
        };

    public static RobotLocation Empty => Create(0, 0);
}