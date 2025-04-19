using Bogus;
using Nerbotix.TestRobotApi.Models;

namespace Nerbotix.TestRobotApi.Jobs;

public class ImitateWorkingJob(IHttpClientFactory httpClientFactory)
{
    public async Task Execute(StartTask task)
    {
        var faker = new Faker();
        var logFaker = new Faker<Log>()
            .RuleFor(l => l.LogLevel, f => f.PickRandom<RoboLogLevel>())
            .RuleFor(l => l.Scope, f => f.PickRandom<LogScope>())
            .RuleFor(l => l.Message, f => f.Random.String2(100))
            .RuleFor(l => l.Timestamp, f => f.Date.RecentOffset().UtcDateTime);
        
        var client = httpClientFactory.CreateClient();
        
        await Task.Delay(task.EstimatedDuration ?? TimeSpan.FromMinutes(1));

        try
        {
            var updateStatus = new UpdateStatus
            {
                TaskId = task.TaskId,
                TaskStatus = faker.PickRandom(RobotTaskStatus.Failed, RobotTaskStatus.Completed),
                LastPosition = new Position
                {
                    Latitude = faker.Address.Latitude(),
                    Longitude = faker.Address.Longitude(),
                },
                Logs = logFaker.Generate(faker.Random.Int(1, 50))
            };
            
            using var response = await client.PutAsJsonAsync($"https://localhost:7152/robots/{task.RobotId}/status", updateStatus);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}