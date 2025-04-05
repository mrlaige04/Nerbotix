namespace RoboTasker.Infrastructure.Hangfire;

public class HangfireDashboardOptions
{
    public const string SectionName = "Hangfire";
    public string Url { get; set; } = null!;
    public string Login  { get; set; } = null!;
    public string Password { get; set; } = null!;
}