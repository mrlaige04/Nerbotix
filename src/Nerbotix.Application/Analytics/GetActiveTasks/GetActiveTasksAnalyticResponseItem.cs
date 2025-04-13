namespace Nerbotix.Application.Analytics.GetActiveTasks;

public class GetActiveTasksAnalyticResponseItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Priority { get; set; }
}