using Hangfire;
using RoboTasker.Application.BackgroundJobs;
using RoboTasker.Infrastructure.Hangfire.Jobs;

namespace RoboTasker.Infrastructure.Hangfire;

public class JobsService(IBackgroundJobClient client) : IJobsService
{
    public string EnqueueTask(Guid taskId)
    {
        return client.Enqueue<TaskAssignJob>(
            job => job.Execute(taskId));
    }
}