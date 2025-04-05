namespace RoboTasker.Infrastructure.Emailing;

public class EmailOptions
{
    public const string SectionName = "Emailing";
    public string From { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}