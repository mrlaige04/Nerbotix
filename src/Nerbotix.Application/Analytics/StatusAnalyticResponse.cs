namespace Nerbotix.Application.Analytics;

public class StatusAnalyticResponse
{
    public IList<StatusAnalyticResponseItem> Items { get; set; } = [];
}

public class StatusAnalyticResponseItem
{
    public string Status { get; set; } = null!;
    public int Count { get; set; }
}