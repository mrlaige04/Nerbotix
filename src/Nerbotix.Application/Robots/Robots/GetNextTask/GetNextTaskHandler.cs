using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.BackgroundJobs;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots.GetNextTask;

public class GetNextTaskHandler(
    IJobsService jobsService,
    ITenantRepository<Robot> robotRepository) : ICommandHandler<GetNextTaskCommand, GetNextTaskCommandResponse>
{
    public async Task<ErrorOr<GetNextTaskCommandResponse>> Handle(GetNextTaskCommand request, CancellationToken cancellationToken)
    {
        var robot = await robotRepository.GetAsync(
            r => r.Id == request.Id,
            q => q.Include(r => r.TasksQueue),
            cancellationToken: cancellationToken);

        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }

        if (robot.TasksQueue.Count == 0)
        {
            return Error.Failure(TaskErrors.NotFound, RobotErrors.NotFoundDescription);
        }

        jobsService.GetNextTask(robot.Id);
        
        return new GetNextTaskCommandResponse(robot.Id);
    }
}