using ErrorOr;
using RoboTasker.Application.BackgroundJobs;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Application.Robots.Tasks.ReEnqueueTask;

public class ReEnqueueTaskHandler(
    ITenantRepository<RobotTask> taskRepository,
    IJobsService jobsService) : ICommandHandler<ReEnqueueTaskCommand>
{
    public async Task<ErrorOr<Success>> Handle(ReEnqueueTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetAsync(
            t => t.Id == request.Id, cancellationToken: cancellationToken);

        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }

        if (task is not { Status: RobotTaskStatus.Pending or RobotTaskStatus.WaitingForEnqueue })
        {
            return Error.Failure(TaskErrors.AlreadyAssigned, TaskErrors.AlreadyAssignedDescription);
        }

        jobsService.EnqueueTask(task.Id);

        return new Success();
    }
}