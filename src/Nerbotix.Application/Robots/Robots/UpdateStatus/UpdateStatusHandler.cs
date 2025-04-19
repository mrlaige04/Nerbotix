using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.BackgroundJobs;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Logging;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Robots.Enums;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Robots.Robots.UpdateStatus;

public class UpdateStatusHandler(
    ITenantRepository<Robot> robotRepository,
    ITenantRepository<RobotTask> taskRepository,
    IJobsService jobsService) : ICommandHandler<UpdateStatusCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetAsync(
            t => t.Id == request.TaskId,
            q => q
                .Include(t => t.AssignedRobot)
                .Include(t => t.Logs),
            cancellationToken: cancellationToken);
        
        var robot = await robotRepository.GetAsync(
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Logs)
                .Include(r => r.TasksQueue),
            cancellationToken: cancellationToken);

        if (task is null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }

        if (task.AssignedRobot is null || task.AssignedRobot.Id != request.Id)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }
        
        if (robot is null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }

        task.Status = request.TaskStatus;
        task.CompletedRobotId = robot.Id;
        
        robot.Status = RobotStatus.Idle;
        robot.LastActivity = DateTime.UtcNow;
        robot.TasksQueue.Remove(task);

        if (request.TaskStatus == RobotTaskStatus.Completed)
        {
            task.CompletedAt = DateTime.UtcNow;
        }

        if (request.LastPosition is not null)
        {
            robot.Location = RobotLocation.Create(
                request.LastPosition.Latitude, 
                request.LastPosition.Longitude);
        }
        
        foreach (var logItem in request.Logs ?? [])
        {
            var log = new Log
            {
                Scope = logItem.Scope,
                Level = logItem.LogLevel,
                Message = logItem.Message,
                Timestamp = logItem.Timestamp
            };

            switch (logItem.Scope)
            {
                case LogScope.Robot:
                    robot.Logs.Add(log);
                    break;
                case LogScope.Task:
                default:
                    task.Logs.Add(log);
                    break;
            }
        }
        
        await robotRepository.UpdateAsync(robot, cancellationToken);
        await taskRepository.UpdateAsync(task, cancellationToken);

        jobsService.GetNextTask(robot.Id);
        
        return new Success();
    }
}