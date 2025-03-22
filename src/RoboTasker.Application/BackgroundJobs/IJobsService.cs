namespace RoboTasker.Application.BackgroundJobs;

public interface IJobsService
{
    string EnqueueTask(Guid taskId);
}