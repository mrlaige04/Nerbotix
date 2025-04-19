using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tasks;

namespace Nerbotix.Application.Robots.Tasks.CancelTask;

public class CancelTaskHandler(
    ITenantRepository<RobotTask> taskRepository) : ICommandHandler<CancelTaskCommand>
{
    public async Task<ErrorOr<Success>> Handle(CancelTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetAsync(
            c => c.Id == request.Id,
            q => q.Include(t => t.Archive),
            cancellationToken: cancellationToken);
        
        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }
        
        task.Status = RobotTaskStatus.Canceled;
        task.AssignedRobotId = null;
        await taskRepository.UpdateAsync(task, cancellationToken);
        
        return new Success();
    }
}