using Hangfire;
using Nerbotix.Application.BackgroundJobs;
using Nerbotix.Infrastructure.Hangfire.Jobs;

namespace Nerbotix.Infrastructure.Hangfire;

public class JobsService(IBackgroundJobClient client) : IJobsService
{
    public string EnqueueTask(Guid taskId)
    {
        return client.Enqueue<TaskDistributeJob>(
            job => job.Execute(taskId));
    }

    public string GetNextTask(Guid robotId)
    {
        return client.Enqueue<GetNextTaskJob>(
            job => job.Execute(robotId));
    }
}