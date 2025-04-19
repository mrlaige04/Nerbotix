namespace Nerbotix.Domain.Robots;

public class RobotLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }

    public static RobotLocation Create(double latitude, double longitude) =>
        new()
        {
            Latitude = latitude,
            Longitude = longitude,
            Timestamp = DateTimeOffset.Now.UtcDateTime
        };

    public static RobotLocation Empty => Create(0, 0);
}